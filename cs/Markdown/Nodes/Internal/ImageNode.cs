using Markdown.Nodes.Interfaces;

namespace Markdown.Nodes.Internal;

public class ImageNode : InternalMarkdownNode
{
    public ImageNode(string value) : base(value)
    {
    }

    public override string ToHtml()
    {
        var alt = Children[0].ToHtml();
        var url = Children[1].ToHtml();
        return $"<img src =\"{url}\" alt=\"{alt}\">";
    }
}