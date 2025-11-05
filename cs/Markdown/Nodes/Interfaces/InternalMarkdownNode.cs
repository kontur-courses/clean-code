namespace Markdown.Nodes.Interfaces;

public abstract class InternalMarkdownNode : MarkdownNode
{
    protected readonly List<MarkdownNode> children = [];
    public override void Add(MarkdownNode node)
    {

    }

    public override List<MarkdownNode> GetChildren()
        => children;

    protected InternalMarkdownNode(MarkdownNode? parent, string value) : base(parent, value)
    {
    }
}