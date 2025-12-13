namespace Markdown.Nodes.Interfaces;

public abstract class InternalMarkdownNode : MarkdownNode
{
    protected InternalMarkdownNode(string value) : base(value)
    {
    }

    public override void AddChild(MarkdownNode node)
    {
        Children.Add(node);
    }

    public override void AddChildren(List<MarkdownNode> nodes)
    {
        Children.AddRange(nodes);
    }
}