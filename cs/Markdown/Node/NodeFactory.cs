namespace Markdown;

public class NodeFactory
{
    public static Node Create(NodeType type, string? info, List<Node>? children)
        => new Node { Type = type, Value = info, Children = children }; 
}