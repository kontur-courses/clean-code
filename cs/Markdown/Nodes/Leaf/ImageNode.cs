using Markdown.Nodes.Interfaces;

namespace Markdown.Nodes.Leaf;

public class ImageNode : LeafMarkdownNode
{
    public override string ToHtml()
    {
        throw new NotImplementedException();
    }

    public ImageNode(MarkdownNode? parent, string value) : base(parent, value)
    {
    }
}