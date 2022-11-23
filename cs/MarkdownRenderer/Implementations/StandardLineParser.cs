using MarkdownRenderer.Abstractions;
using MarkdownRenderer.Abstractions.Elements;
using MarkdownRenderer.Abstractions.ElementsParsers;
using MarkdownRenderer.Infrastructure;

namespace MarkdownRenderer.Implementations;

public class DefaultLineParser : ILineParser
{
    private readonly IEscapeSequenceElementParser? _escapeSequenceParser;

    private readonly IInlineElementParser _defaultInlineElementParser;
    private readonly IReadOnlyCollection<ISpecialInlineElementParser> _specialInlineElementsParsers;

    private readonly ILineElementParser _defaultLineElementParser;
    private readonly IReadOnlyCollection<ISpecialLineElementParser> _specialLineElementsParsers;

    private readonly IElementsNestingRules _nestingRules;

    public DefaultLineParser(IEnumerable<IElementParser> parsers, IElementsNestingRules nestingRules)
    {
        _nestingRules = nestingRules;

        var inlineSpecialParsers = new HashSet<ISpecialInlineElementParser>();
        var lineSpecialParsers = new HashSet<ISpecialLineElementParser>();
        foreach (var parser in parsers)
        {
            switch (parser)
            {
                case IEscapeSequenceElementParser escapeSequenceParser:
                    if (_escapeSequenceParser is not null)
                        throw new ArgumentException("Should be only one escape sequence parser!");
                    _escapeSequenceParser = escapeSequenceParser;
                    break;
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

        if (_escapeSequenceParser is not null)
            SetEscapingSequences();
    }

    private void SetEscapingSequences()
    {
        var escapingSequences = _specialInlineElementsParsers
            .SelectMany(parser => new[] {parser.Prefix, parser.Postfix})
            .Concat(_specialLineElementsParsers.SelectMany(parser => new[] {parser.Prefix, parser.Postfix}))
            .Where(sequence => !string.IsNullOrWhiteSpace(sequence));

        _escapeSequenceParser?.SetEscapingSequences(escapingSequences);
    }

    public IElement ParseLineContent(string content)
    {
        var lineElement = ParseLineElement(content, out var inlineContentToken);

        ParseInlineElements(lineElement, content.Substring(inlineContentToken));
        return lineElement;
    }

    private IElement ParseLineElement(string content, out ContentToken inlineContentToken)
    {
        inlineContentToken = new ContentToken(0, content.Length - 1, 0, 0);
        var specialLineParser = _specialLineElementsParsers.FirstOrDefault(parser => parser.Match(content));
        if (specialLineParser is null)
            return _defaultLineElementParser.ParseElement(content);

        inlineContentToken = new ContentToken(
            0, content.Length - 1,
            specialLineParser.Prefix.Length, specialLineParser.Postfix.Length
        );
        return specialLineParser.ParseElement(content);
    }

    private void ParseInlineElements(IElement lineElement, string content)
    {
        var closedTokens = ParseTokens(content);

        foreach (var intersecting in GetIntersections(closedTokens.Keys).ToHashSet())
            closedTokens.Remove((ContentToken) intersecting);

        foreach (var nestedElement in ParseNestedElements(content, closedTokens, lineElement))
            lineElement.AddNested(nestedElement);
    }

    private SortedDictionary<ContentToken, IElement> ParseTokens(string content)
    {
        var openedParsers = new Dictionary<ISpecialInlineElementParser, int>();
        var closedTokens = new SortedDictionary<ContentToken, IElement>(
            new LambdaComparer<ContentToken>((token1, token2) => token1.Start - token2.Start));

        for (var i = 0; i < content.Length; i++)
        {
            if (TryParseEscapeSequence(closedTokens, content, i, out var sequenceToken))
                i = sequenceToken!.ContentEnd;
            else if (TryCloseParser(openedParsers, closedTokens, content, i, out var closedParser))
                i += closedParser!.Postfix.Length - 1;
            else if (TryOpenParser(content, i, openedParsers, out var openedParser))
                i += openedParser!.Prefix.Length - 1;
        }

        return closedTokens;
    }

    private bool TryParseEscapeSequence(
        IDictionary<ContentToken, IElement> closedTokens,
        string content, int i,
        out ContentToken? sequenceToken
    )
    {
        sequenceToken = default;
        if (
            _escapeSequenceParser is null ||
            !_escapeSequenceParser.TryGetEscapingSequenceToken(content, i, out sequenceToken)
        )
            return false;

        closedTokens[sequenceToken!] = _escapeSequenceParser.ParseElement(content, sequenceToken!);
        return true;
    }

    private bool TryCloseParser(
        IDictionary<ISpecialInlineElementParser, int> openedParsers,
        IDictionary<ContentToken, IElement> closedTokens,
        string content, int i, out ISpecialInlineElementParser? closedParser
    )
    {
        closedParser = openedParsers.Keys.FirstOrDefault(parser => parser.IsElementEnd(content, i));
        if (closedParser is null)
            return false;

        var start = openedParsers[closedParser];
        var closedToken = new ContentToken(
            start, i,
            closedParser.Prefix.Length, closedParser.Postfix.Length
        );

        if (!closedParser.TryParseElement(content, closedToken, out var element))
            return false;

        openedParsers.Remove(closedParser);
        closedTokens[closedToken] = element!;
        return true;
    }

    private bool TryOpenParser(
        string content, int i,
        IDictionary<ISpecialInlineElementParser, int> openedParsers,
        out ISpecialInlineElementParser? openedParser
    )
    {
        openedParser = _specialInlineElementsParsers
            .Where(parser => parser.IsElementStart(content, i))
            .DefaultIfEmpty()
            .MaxBy(parser => parser?.Prefix.Length ?? 0);

        if (openedParser is null || openedParsers.ContainsKey(openedParser))
            return false;

        openedParsers[openedParser] = i;
        return true;
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

    private IEnumerable<IElement> ParseNestedElements(
        string content,
        IReadOnlyDictionary<ContentToken, IElement> tokens,
        IElement parent,
        int parseStart = 0, int? parseEnd = null
    )
    {
        parseEnd ??= content.Length - 1;

        foreach (var (token, element) in tokens)
        {
            if (token.Start < parseStart || !_nestingRules.CanContainNested(parent, element))
                continue;
            if (token.End > parseEnd)
                break;

            if (TryParseDefElement(content, parseStart, token.Start - 1, parent, out var defElement))
                yield return defElement!;

            foreach (var nested in ParseNestedElements(content, tokens, element, token.ContentStart, token.ContentEnd))
                element.AddNested(nested);

            yield return element;

            parseStart = token.End + 1;
        }

        if (TryParseDefElement(content, parseStart, parseEnd.Value, parent, out var result))
            yield return result!;
    }

    private bool TryParseDefElement(string content, int start, int end, IElement parent, out IElement? element)
    {
        element = default;
        if (!_nestingRules.CanContainNested(parent, _defaultInlineElementParser.ParsingElementType) || start > end)
            return false;
        element = _defaultInlineElementParser.ParseElement(content, new Token(start, end));
        return true;
    }
}