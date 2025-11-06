namespace Markdown.Node;

public interface INode
{
    IEnumerable<INode> Children { get; }
}