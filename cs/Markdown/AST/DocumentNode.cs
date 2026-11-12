namespace Markdown.AST;

public sealed class DocumentNode : MarkdownNode
{
    public List<BlockNode> Children { get; } = [];
}
