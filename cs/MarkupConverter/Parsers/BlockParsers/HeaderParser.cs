using MarkupConverter.Parsers.BlockParsers.OpenBlocks;

namespace MarkupConverter.Parsers.BlockParsers;

public class HeaderParser : IBlockParser
{
    public bool CanParse(string line)
    {
        var trimmed = line.TrimStart();
        var headingLevel = CountHeadingLevel(trimmed);

        return headingLevel is >= 1 and <= 6
               && (headingLevel == trimmed.Length || char.IsWhiteSpace(trimmed[headingLevel]));
    }

    public IOpenBlock Parse(string line)
    {
        var trimmed = line.TrimStart();
        var headingLevel = CountHeadingLevel(trimmed);
        var content = trimmed.Substring(headingLevel + 1).Trim();

        return new HeaderOpenBlock(content, headingLevel);
    }

    private static int CountHeadingLevel(string line)
    {
        var level = 0;
        foreach (var c in line)
            if (c == '#') level++;
            else break;

        return level;
    }
}