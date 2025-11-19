using System.Text;
using Markdown.Nodes.Interfaces;

namespace Markdown.Nodes.Internal;

public class HeaderNode : InternalMarkdownNode
{
    public HeaderNode(MarkdownNode? parent, string value) : base(parent, value)
    {
    }
    
    public override string ToHtml()
    {
        var textBuilder = new StringBuilder();
        foreach (var child in children)
        {
            textBuilder.Append(child.ToHtml());
        }

        return $"<h1>{textBuilder}</h1>";
    }
}