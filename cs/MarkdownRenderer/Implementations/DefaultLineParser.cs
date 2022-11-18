using MarkdownRenderer.Abstractions;
using MarkdownRenderer.Abstractions.Elements;
using MarkdownRenderer.Abstractions.ElementsParsers;
using MarkdownRenderer.Infrastructure;

namespace MarkdownRenderer.Implementations;

public class DefaultLineParser : ILineParser
{
    private readonly IInlineElementParser _defaultInlineElementParser;
    private readonly IReadOnlyCollection<ISpecialInlineElementParser> _specialInlineElementsParsers;

    private readonly ILineElementParser _defaultLineElementParser;
    private readonly IReadOnlyCollection<ISpecialLineElementParser> _specialLineElementsParsers;

    public DefaultLineParser(IEnumerable<IElementParser> parsers)
    {
        var inlineSpecialParsers = new HashSet<ISpecialInlineElementParser>();
        var lineSpecialParsers = new HashSet<ISpecialLineElementParser>();
        foreach (var parser in parsers)
        {
            switch (parser)
            {
                case ISpecialInlineElementParser specialInlineParser:
                    inlineSpecialParsers.Add(specialInlineParser);
                    break;
                case ISpecialLineElementParser specialLineParser:
                    lineSpecialParsers.Add(specialLineParser);
                    break;
                case IInlineElementParser defaultInlineParser:
                    if (_defaultInlineElementParser is not null)
                        throw new ArgumentException("Should be only one default inline element parser!");
                    _defaultInlineElementParser = defaultInlineParser;
                    break;
                case ILineElementParser defaultLineParser:
                    if (_defaultLineElementParser is not null)
                        throw new ArgumentException("Should be only one default line element parser!");
                    _defaultLineElementParser = defaultLineParser;
                    break;
            }
        }

        if (_defaultInlineElementParser is null || _defaultLineElementParser is null)
            throw new ArgumentException("Default parsers should be assigned!");
        _specialInlineElementsParsers = inlineSpecialParsers;
        _specialLineElementsParsers = lineSpecialParsers;
    }

    public IElement ParseContentLine(string content)
    {
        var specialLineParser = _specialLineElementsParsers.FirstOrDefault(parser => parser.Match(content));
        var line = specialLineParser is null
            ? _defaultLineElementParser.ParseElement(content)
            : specialLineParser.ParseElement(content);

        ParseInlineElements(line);
        return line;
    }

    private void ParseInlineElements(IElement lineElement)
    {
        var content = lineElement.RawContent;

        var openedParsers = new Dictionary<ISpecialInlineElementParser, int>();
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

        ParseNestedElements(lineElement, content, closedTokens);
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