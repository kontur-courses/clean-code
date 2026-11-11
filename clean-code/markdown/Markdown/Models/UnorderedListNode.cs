namespace Markdown.Models
{
	public class UnorderedListNode : INode
	{
		public IReadOnlyList<INode> Children { get; }

		public UnorderedListNode(IReadOnlyList<INode> children)
		{
			Children = children;
		}
	}
}
