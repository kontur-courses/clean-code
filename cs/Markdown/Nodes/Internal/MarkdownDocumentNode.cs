using Markdown.Nodes.Interfaces;

namespace Markdown.Nodes.Internal;

public class MarkdownDocumentNode : InternalMarkdownNode
{
    public MarkdownDocumentNode(MarkdownNode? parent, string value) : base(parent, value)
    {
    }

    public override string ToHtml()
    {
        throw new NotImplementedException();
    }

}