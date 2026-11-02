namespace Markdown.Nodes;

public abstract record InlineNode;

public record TextNode(
    string Text
) : InlineNode;

public record EmphasisNode(
    IReadOnlyList<InlineNode> Children
) : InlineNode;

public record StrongNode(
    IReadOnlyList<InlineNode> Children
) : InlineNode;