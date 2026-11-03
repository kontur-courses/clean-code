namespace Markdown.Models
{
	public class EmNode : INode
	{
		public IReadOnlyList<INode> Children { get; }

		public EmNode(IReadOnlyList<INode> children)
		{
			// TODO
			throw new NotImplementedException();
		}
	}
}
