namespace Markdown.AST;

public sealed class HeadingNode : BlockNode
{
    public int Level { get; init; } = 1;
    public List<InlineNode> Children { get; } = [];
}
