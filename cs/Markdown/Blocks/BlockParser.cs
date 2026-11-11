namespace Markdown.Blocks;

public class BlockParser
{
    private const string HeaderMarker = "# ";

    public IReadOnlyList<Block> Parse(IReadOnlyList<string> lines)
    {
        var blocks = new List<Block>();
        var blockContent = new List<string>();

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                AddBlock(blocks, blockContent);
                continue;
            }

            blockContent.Add(line);
        }

        AddBlock(blocks, blockContent);
        return blocks;
    }

    private static void AddBlock(List<Block> blocks, List<string> current)
    {
        if (current.Count == 0)
            return;

        var firstLine = current[0];
        var backslashCount = 0;
        while (backslashCount < firstLine.Length && firstLine[backslashCount] == '\\')
            backslashCount++;

        var hasHeaderMarker = firstLine[backslashCount..].StartsWith(HeaderMarker, StringComparison.Ordinal);
        var isHeader = backslashCount == 0 && hasHeaderMarker;
        var isEscapedHeader = backslashCount % 2 == 1 && hasHeaderMarker;

        if (isHeader)
            current[0] = firstLine[HeaderMarker.Length..];
        else if (isEscapedHeader)
            current[0] = firstLine[1..];

        blocks.Add(new Block(
            isHeader ? BlockKind.Header : BlockKind.Paragraph,
            string.Join("\n", current)));

        current.Clear();
    }
}