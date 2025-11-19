using Markdown.Nodes.Interfaces;

namespace Markdown.Nodes.Internal;

public class ImageNode : InternalMarkdownNode
{
    public ImageNode(MarkdownNode? parent, string value) : base(parent, value)
    {
    }

    public override string ToHtml()
    {
        var alt = children[0].ToHtml();
        var url = children[1].ToHtml();
        return $"<img src =\"{url}\" alt=\"{alt}\">";
    } 
}