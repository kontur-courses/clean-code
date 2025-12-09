using MarkupConverter.Parsers.BlockParsers.OpenBlocks;

namespace MarkupConverter.Parsers.BlockParsers;

public class ParagraphParser : IBlockParser
{
    public bool CanParse(string line)
    {
        return !string.IsNullOrWhiteSpace(line);
    }

    public IOpenBlock Parse(string line)
    {
        var content = line.Trim();

        return new ParagraphOpenBlock(content);
    }
}