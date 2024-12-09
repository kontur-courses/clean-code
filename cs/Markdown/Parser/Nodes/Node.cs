namespace Markdown.Parser.Nodes;

public record Node(NodeType NodeType, int Start, int Consumed)
{
    public int End { get; } = Start + Consumed - 1;
}