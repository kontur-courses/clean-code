namespace Markdown.Models
{
	// Узел разметки
	public interface INode
	{
		IReadOnlyList<INode> Children { get; }
	}
}
