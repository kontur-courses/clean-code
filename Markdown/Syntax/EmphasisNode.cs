namespace Markdown.Syntax;

public sealed record EmphasisNode(IReadOnlyList<InlineNode> Children) : InlineNode;
