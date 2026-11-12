namespace Markdown.AST;

public sealed class StrongNode : InlineNode
{
    public List<InlineNode> Children { get; } = [];
}