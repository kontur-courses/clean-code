using MarkupConverter.AST.Inlines;

namespace MarkupConverter.Parsers.InlineParsers;

public interface IInlineParser
{
    public bool CanParse(string text);
    public Inline Parse(string text);
}