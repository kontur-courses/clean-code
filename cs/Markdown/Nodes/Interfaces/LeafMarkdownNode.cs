namespace Markdown.Nodes.Interfaces;

public abstract class LeafMarkdownNode : MarkdownNode
{
    protected LeafMarkdownNode(MarkdownNode? parent, string value) : base(parent, value)
    {
    }
    
    public override void AddChild(MarkdownNode node)
    {
        throw new NotImplementedException();
    }
}