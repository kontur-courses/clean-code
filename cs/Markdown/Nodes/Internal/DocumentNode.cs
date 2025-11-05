using Markdown.Nodes.Interfaces;

namespace Markdown.Nodes.Internal;

public class DocumentNode : InternalMarkdownNode
{
    public DocumentNode(MarkdownNode? parent, string value) : base(parent, value)
    {
    }

    public override string ToHtml()
        => "";

}