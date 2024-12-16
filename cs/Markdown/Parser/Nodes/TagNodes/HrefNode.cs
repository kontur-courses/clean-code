namespace Markdown.Parser.Nodes.TagNodes;

public record HrefNode(string Href, List<Node> Children, int Start, int Consumed)
    : TagNode(NodeType.HREF, Children, Start, Consumed);