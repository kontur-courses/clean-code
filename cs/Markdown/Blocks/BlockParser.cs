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

        var isHeader = current[0].StartsWith(HeaderMarker, StringComparison.Ordinal);

        if (isHeader)
            current[0] = current[0][HeaderMarker.Length..];

        blocks.Add(new Block(
            isHeader ? BlockKind.Header : BlockKind.Paragraph,
            string.Join("\n", current)));

        current.Clear();
    }
}