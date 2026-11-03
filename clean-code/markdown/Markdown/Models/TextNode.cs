namespace Markdown.Models
{
	public class TextNode : INode
	{
		public string Value { get; }
		public IReadOnlyList<INode> Children => Array.Empty<INode>();

		public TextNode(string value)
		{
			// TODO
			throw new NotImplementedException();
		}
	}
}
