namespace Markdown.Parser.Nodes;

public class TagNode(NodeType nodeType, List<Node> children, int consumed) : Node(nodeType, consumed)
{
    public List<Node> Children { get; } = children;

    public TagNode(NodeType nodeType, Node child, int consumed) :
        this(nodeType, [child], consumed)
    {
    }
}