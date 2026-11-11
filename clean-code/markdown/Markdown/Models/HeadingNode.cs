namespace Markdown.Models
{
	public class HeadingNode : INode
	{
		public IReadOnlyList<INode> Children { get; }
		
		public HeadingNode(IReadOnlyList<INode> children)
		{
			Children = children;
		}
	}
}
