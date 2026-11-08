namespace Markdown.Models
{
	public class StrongNode : INode
	{
		public IReadOnlyList<INode> Children { get; }

		public StrongNode(IReadOnlyList<INode> children)
		{
			Children = children;
		}
	}
}
