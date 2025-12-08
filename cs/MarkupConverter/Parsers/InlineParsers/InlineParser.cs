using MarkupConverter.AST.Inlines;

namespace MarkupConverter.Parsers.InlineParsers;

public class InlineParser : IInlineParser
{
    public List<Inline> Parse(string text)
    {
        return new List<Inline> { new Text(text) };
    }
}