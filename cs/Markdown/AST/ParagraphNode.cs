namespace Markdown.AST;

public sealed class ParagraphNode : BlockNode
{
    public List<InlineNode> Children { get; } = [];
}