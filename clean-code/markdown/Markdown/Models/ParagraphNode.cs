namespace Markdown.Models
{
	public class ParagraphNode : INode
	{
		public IReadOnlyList<INode> Children { get; }

		public ParagraphNode(IReadOnlyList<INode> children)
		{
			Children = children;
		}
	}
}
