namespace Markdown.TreeBuilder.Nodes;

public class BoldNode : Node
{
	public override string OpenTag => "<strong>";
	public override string CloseTag => "</strong>";
}