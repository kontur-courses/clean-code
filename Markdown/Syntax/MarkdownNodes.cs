namespace Markdown.Syntax;

internal abstract record MarkdownNode;

internal abstract record BlockNode : MarkdownNode;

internal abstract record InlineNode : MarkdownNode;

internal sealed record DocumentNode(
    IReadOnlyList<BlockNode> Blocks
) : MarkdownNode;

internal sealed record HeadingNode(
    IReadOnlyList<InlineNode> Children
) : BlockNode;

internal sealed record ParagraphNode(
    IReadOnlyList<InlineNode> Children
) : BlockNode;

internal sealed record UnorderedListNode(
    IReadOnlyList<ListItemNode> Items
) : BlockNode;

internal sealed record ListItemNode(
    IReadOnlyList<InlineNode> Children
) : MarkdownNode;

internal sealed record TextNode(
    string Text
) : InlineNode;

internal sealed record EmphasisNode(
    IReadOnlyList<InlineNode> Children
) : InlineNode;

internal sealed record StrongNode(
    IReadOnlyList<InlineNode> Children
) : InlineNode;