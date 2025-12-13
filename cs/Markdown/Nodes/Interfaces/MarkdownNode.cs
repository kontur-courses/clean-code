namespace Markdown.Nodes.Interfaces;

public abstract class MarkdownNode
{
    public readonly List<MarkdownNode> Children = [];
    public readonly string Value;

    protected MarkdownNode(string value)
    {
        Value = value;
    }

    public virtual void AddChild(MarkdownNode node)
    {
    }

    public virtual void AddChildren(List<MarkdownNode> nodes)
    {
    }

    public abstract string ToHtml();
}