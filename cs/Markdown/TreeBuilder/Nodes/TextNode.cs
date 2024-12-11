namespace Markdown.TreeBuilder.Nodes;

public class TextNode : Node
{
	public string? Value { get; init; } = string.Empty;
}