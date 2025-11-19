using System.Text;

namespace Markdown.Nodes.Interfaces;

public abstract class InternalMarkdownNode : MarkdownNode
{
    protected readonly List<MarkdownNode> children = [];
    
    protected InternalMarkdownNode(MarkdownNode? parent, string value) : base(parent, value)
    { }
    
    public override void AddChild(MarkdownNode node)
    {
        children.Add(node);
    }
    
}