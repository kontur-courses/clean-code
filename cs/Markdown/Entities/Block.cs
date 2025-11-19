namespace Markdown.Entities;

public class Block(string rawText, BlockType type)
{
    public string RawText { get; } = rawText;

    public BlockType Type { get; } = type;

    public IReadOnlyList<Node> Inlines { get; init; } = [];
}

public enum BlockType
{
    Heading,
    Paragraph
}