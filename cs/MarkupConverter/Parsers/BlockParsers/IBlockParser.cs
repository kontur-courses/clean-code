using MarkupConverter.AST.Blocks;

namespace MarkupConverter.Parsers.BlockParsers;

public interface IBlockParser
{
    public bool CanParse(string text);
    public Block Parse(string text);
}