namespace Markdown.Node;

public class TextNode : INode
{
    public IEnumerable<INode> Children { get; }
}