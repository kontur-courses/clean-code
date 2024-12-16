namespace Markdown.Parser.Nodes.TagNodes;

public record HeadlineNode(int Level, List<Node> Children, int Start, int Consumed) 
    : TagNode(NodeType.HEADLINE, Children, Start, Consumed);
