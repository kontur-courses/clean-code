using MarkupConverter.AST.Blocks;

namespace MarkupConverter.Parsers;

public interface IParser
{
    public Document Parse(string text);
}