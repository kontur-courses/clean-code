using MarkdownRenderer.Abstractions;

namespace MarkdownRenderer.Implementations;

public class DocumentParser
{
    private readonly DefaultElementParser _defaultInlineElementParser;
    private readonly HashSet<IElementParser> _specialInlineElementsParsers;

    private readonly DefaultElementParser _defaultLineElementParser;
    private readonly HashSet<IElementParser> _specialLineElementsParsers;

    public DocumentParser(IEnumerable<IElementParser> parsers)
    {
        var parsersDict = parsers
            .GroupBy(parser => parser.ParseType)
            .ToDictionary(group => group.Key);

        _specialInlineElementsParsers = parsersDict[ElementParseType.Inline].ToHashSet();
        _defaultInlineElementParser = (DefaultElementParser) _specialInlineElementsParsers
            .Single(parser => parser is DefaultElementParser);
        _specialInlineElementsParsers.Remove(_defaultInlineElementParser);

        _specialLineElementsParsers = parsersDict[ElementParseType.Line].ToHashSet();
        _defaultLineElementParser = (DefaultElementParser) _specialLineElementsParsers
            .Single(parser => parser is DefaultElementParser);
        _specialLineElementsParsers.Remove(_defaultLineElementParser);
    }

    public IElement ParseContent(string content)
    {
        var line = _defaultLineElementParser.ParseElement(content, new Token(0, content.Length - 1));
        ParseInlineElements(content, line);
        return line;
    }

    private void ParseInlineElements(string content, IElement parent)
    {
        var openedParsers = new Dictionary<IElementParser, int>();
        var closedTokens = new SortedDictionary<ContentToken, IElement>(
            new LambdaComparer<Token>((token1, token2) => token1.Start - token2.Start));

        for (var i = 0; i < content.Length; i++)
        {
            var closedParser = openedParsers.Keys.FirstOrDefault(parser => parser.IsElementEnd(content, i));
            if (closedParser is not null)
            {
                var start = openedParsers[closedParser];
                var closedToken = new ContentToken(
                    start, i,
                    start + closedParser.Prefix.Length,
                    i - closedParser.Postfix.Length
                );
                if (closedParser.TryParseElement(content, closedToken, out var element))
                {
                    closedTokens[closedToken] = element!;
                    openedParsers.Remove(closedParser);
                    continue;
                }
            }

            var opened = _specialInlineElementsParsers.FirstOrDefault(parser => parser.IsElementStart(content, i));
            if (opened is not null && !openedParsers.ContainsKey(opened))
                openedParsers[opened] = i;
        }

        foreach (var intersecting in GetIntersections(closedTokens.Keys).ToHashSet())
            closedTokens.Remove((ContentToken) intersecting);

        ParseNestedElements(parent, content, closedTokens);
    }


    private static IEnumerable<Token> GetIntersections(IReadOnlyCollection<Token> startSortedTokens)
    {
        foreach (var token1 in startSortedTokens)
        {
            foreach (var token2 in startSortedTokens)
            {
                if (token1 == token2)
                    break;
                if (token2.End < token1.Start || token2.End > token1.End)
                    continue;

                yield return token1;
                yield return token2;
            }
        }
    }

    private void ParseNestedElements(
        IElement parent,
        string content,
        IReadOnlyDictionary<ContentToken, IElement> tokens,
        int start = 0, int? parentEnd = null
    )
    {
        parentEnd ??= content.Length - 1;

        foreach (var (token, element) in tokens)
        {
            if (token.Start < start || !parent.CanContainNested(element.GetType()))
                continue;
            if (token.End > parentEnd)
                break;

            if (TryParseDefaultElement(content, start, token.Start - 1, out var defElement))
                parent.AddNestedElement(defElement!);

            parent.AddNestedElement(element);
            ParseNestedElements(element, content, tokens, token.ContentStart, token.ContentEnd);
            start = token.End + 1;
        }

        if (TryParseDefaultElement(content, start, parentEnd.Value, out var result))
            parent.AddNestedElement(result!);
    }

    private bool TryParseDefaultElement(string content, int start, int end, out IElement? element)
    {
        element = null;
        if (start > end)
            return false;
        element = _defaultInlineElementParser.ParseElement(content, new Token(start, end));
        return true;
    }
}