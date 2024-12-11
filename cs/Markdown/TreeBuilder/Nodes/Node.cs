namespace Markdown.TreeBuilder.Nodes;

public abstract class Node
{
	public List<Node> Children { get; } = new();
	public Node? Parent { get; set; }

	public virtual string OpenTag => string.Empty;
	public virtual string CloseTag => string.Empty;
}