namespace Markdown.Syntax;

public sealed record StrongNode(IReadOnlyList<InlineNode> Children) : InlineNode;