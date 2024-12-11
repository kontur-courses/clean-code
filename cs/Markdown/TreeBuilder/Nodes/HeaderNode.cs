namespace Markdown.TreeBuilder.Nodes;

public class HeaderNode : Node
{
	public override string OpenTag => "<h1>";
	public override string CloseTag => "</h1>";
}