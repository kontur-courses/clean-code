using Markdown.Nodes.Interfaces;

namespace Markdown.Nodes.Internal;

public class BoldNode : InternalMarkdownNode
{
    public override string ToHtml()
    {
        throw new NotImplementedException();
    }

    public BoldNode(MarkdownNode? parent, string value) : base(parent, value)
    {
    }
}