namespace Markdown.AST;

public sealed class EmphasisNode : InlineNode
{
    public List<InlineNode> Children { get; } = [];
}
