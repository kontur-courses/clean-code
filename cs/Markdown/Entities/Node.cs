namespace Markdown.Entities;

public class Node(string? text, NodeType type)
{
    public string? Text { get; } = text;

    public NodeType Type { get; } = type;
}

public enum NodeType
{
    Text,
    Strong,
    Em
}