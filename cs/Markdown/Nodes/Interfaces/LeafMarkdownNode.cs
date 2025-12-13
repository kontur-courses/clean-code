namespace Markdown.Nodes.Interfaces;

public abstract class LeafMarkdownNode : MarkdownNode
{
    protected LeafMarkdownNode(string value) : base(value)
    {
    }
}