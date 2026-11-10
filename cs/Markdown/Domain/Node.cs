namespace Markdown.Domain;

public abstract record Node;

public abstract record ContainerNode : Node
{
    public IReadOnlyList<Node> Children { get; init; } = [];
}

public record TextNode(string Text) : Node;

public record ParagraphNode : ContainerNode;
public record HeaderNode : ContainerNode;
public record EmphasisNode : ContainerNode;
public record StrongNode : ContainerNode;