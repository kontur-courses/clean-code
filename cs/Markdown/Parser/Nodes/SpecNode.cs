namespace Markdown.Parser.Nodes;

public class SpecNode(List<Node> children, int consumed) : Node(NodeType.Special, consumed)
{
    public List<Node> Children { get; } = children;
}