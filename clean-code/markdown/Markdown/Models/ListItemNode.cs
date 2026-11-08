namespace Markdown.Models
{
	public class ListItemNode : INode
	{
		public IReadOnlyList<INode> Children { get; }

		public ListItemNode(IReadOnlyList<INode> children)
		{
			Children = children;
		}
	}
}
