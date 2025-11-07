namespace Markdown.Nodes.Interfaces;

public abstract class MarkdownNode
{   
    public MarkdownNode? Parent;
    protected string value;

    protected MarkdownNode(MarkdownNode? parent, string value)
    {
        Parent = parent;
        this.value = value;
    }

    public virtual void AddChild(MarkdownNode node) {}
    public virtual List<MarkdownNode> GetChildren() => [];
    public abstract string ToHtml();
}