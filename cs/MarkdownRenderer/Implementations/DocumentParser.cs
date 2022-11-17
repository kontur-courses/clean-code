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
        ParseNestedElements(line);
        return line;
    }

    private void ParseNestedElements(IElement parent)
    {
        var content = parent.RawContent;
        var start = 0;
        while (start < content.Length)
        {
            // var parser = _specialInlineElementsParsers
            //     .FirstOrDefault(parser => parser.IsElementStart(content, start));
            // if (
            //     parser is null ||
            //     !content.TryReadToken(start, parser.IsElementEnd, out var token) ||
            //     !parser.TryParseElement(content, token!, out var el)
            // )
            // {
            //     token = content.TryReadToken(
            //         start,
            //         (s, i) => _specialInlineElementsParsers.Any(p => p.IsElementStart(s, i)),
            //         out token
            //     )
            //         ? new Token(start, token!.End - 1)
            //         : new Token(start, content.Length - 1);
            //     var element = _defaultInlineElementParser.ParseElement(content, token);
            //     start = token.End + 1;
            //     parent.AddNestedElement(element);
            // }
            // else
            // {
            //     parent.AddNestedElement(el!);
            //     ParseNestedElements(el!);
            //     start = token!.End + 1;
            // }

            if (TryParseSpecialElement(content, start, parent, out var element, out var rawLength))
            {
                ParseNestedElements(element!);
            }
            else
            {
                element = ParseDefaultElement(content, start, parent, out rawLength);
            }

            parent.AddNestedElement(element!);
            start += (int) rawLength!;
        }
    }

    private bool TryParseSpecialElement(
        string content, int start,
        IElement parent,
        out IElement? element,
        out int? rawLength
    )
    {
        element = null;
        rawLength = null;

        var parser = _specialInlineElementsParsers.FirstOrDefault(parser => parser.IsElementStart(content, start));
        if (parser is null || !parent.CanContainNested(parser.ParsingElementType))
            return false;

        if (
            !content.TryReadToken(start, parser.IsElementEnd, out var token) ||
            !parser.TryParseElement(content, token!, out element)
        )
            return false;

        rawLength = token!.Length;
        return true;
    }

    private IElement ParseDefaultElement(string content, int start, IElement parent, out int? rawLength)
    {
        if (!content.TryReadToken(start, (s, i) => IsSpecialElementStart(s, i, parent), out var token))
            token = new Token(start, content.Length);
        token = new Token(token!.Start, token.End - 1);
        rawLength = token.Length;
        return _defaultInlineElementParser.ParseElement(content, token);
    }

    private bool IsSpecialElementStart(string content, int i, IElement parent)
    {
        var parser = _specialInlineElementsParsers.FirstOrDefault(p => p.IsElementStart(content, i));
        return parser is not null && parent.CanContainNested(parser.ParsingElementType);
    }
}