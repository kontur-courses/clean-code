using MarkupConverter.Parsers.BlockParsers.OpenBlocks;

namespace MarkupConverter.Parsers.BlockParsers;

public interface IBlockParser
{
    bool CanParse(string line);
    IOpenBlock Parse(string line);
}