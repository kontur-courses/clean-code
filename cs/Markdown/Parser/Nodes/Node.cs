using Markdown.Tokens;

namespace Markdown.Parser.Nodes;

public class Node(NodeType type, List<Token> value, int consumed)
{
    public int Consumed { get; } = consumed;
    public List<Token> Value { get; } = value;
    public NodeType Type { get; } = type;

}