namespace NewMarkdown;

public class Node
{
	public string? Text { get; set; }
	public NodeType Type { get; set; }
	public List<Node>? Children { get; set; }

	public Node(NodeType type, string? text = null)
	{
		Type = type;
		Text = text;
	}
}