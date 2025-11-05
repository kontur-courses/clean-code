using Markdown.Nodes.Interfaces;

namespace Markdown.Nodes.Internal;

public class HeaderNode : InternalMarkdownNode
{
    public override string ToHtml()
    {
        throw new NotImplementedException();
    }

    public HeaderNode(MarkdownNode? parent, string value) : base(parent, value)
    {
    }
}