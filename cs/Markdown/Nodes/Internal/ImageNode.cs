using Markdown.Nodes.Interfaces;

namespace Markdown.Nodes.Internal;

public class ImageNode : InternalMarkdownNode
{
    public override string ToHtml()
    {
        throw new NotImplementedException();
    }

    public ImageNode(MarkdownNode? parent, string value) : base(parent, value)
    {
    }
}