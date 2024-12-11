namespace Markdown.TreeBuilder.Nodes;

public class ItalicNode : Node
{
	public override string OpenTag => "<em>";
	public override string CloseTag => "</em>";
}