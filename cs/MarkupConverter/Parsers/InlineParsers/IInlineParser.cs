using MarkupConverter.AST.Inlines;

namespace MarkupConverter.Parsers.InlineParsers;

public interface IInlineParser
{
    public List<Inline> Parse(string text);
}