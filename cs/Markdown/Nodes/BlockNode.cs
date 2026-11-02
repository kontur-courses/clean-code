namespace Markdown.Nodes;

public abstract record BlockNode();

public record ParagraphNode(
    IReadOnlyList<InlineNode> Children
) : BlockNode;

public record HeaderNode(
    IReadOnlyList<InlineNode> Children
) : BlockNode;