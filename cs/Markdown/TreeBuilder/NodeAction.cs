using Markdown.TreeBuilder.Nodes;

namespace Markdown.TreeBuilder;

public abstract class NodeAction
{
	public class OpenNode : NodeAction
	{
		public Node Node { get; }
		public OpenNode(Node node) => Node = node;
	}

	public class CloseNode : NodeAction
	{
	}

	public class SkipNode : NodeAction
	{
	}
}