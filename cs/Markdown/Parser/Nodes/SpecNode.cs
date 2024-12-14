namespace Markdown.Parser.Nodes;

public record SpecNode(List<Node> Nodes, int Start, int Consumed) : Node(NodeType.Special, Start, Consumed);