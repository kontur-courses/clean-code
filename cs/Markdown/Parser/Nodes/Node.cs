using Markdown.Tokens;

namespace Markdown.Parser.Nodes;

public class Node(NodeType nodeType, int consumed)
{
    public int Consumed { get; } = consumed;
    public NodeType NodeType { get; } = nodeType;

}