using Markdown.Nodes.Interfaces;

namespace Markdown.Nodes.Internal;

public class ItalicNode : InternalMarkdownNode
{
    public override string ToHtml()
    {
        throw new NotImplementedException();
    }

    public ItalicNode(MarkdownNode? parent, string value) : base(parent, value)
    {
    }
}