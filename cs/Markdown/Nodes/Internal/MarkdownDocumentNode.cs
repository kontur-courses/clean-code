using System.Text;
using Markdown.Nodes.Interfaces;

namespace Markdown.Nodes.Internal;

public class MarkdownDocumentNode : InternalMarkdownNode
{
    public MarkdownDocumentNode(MarkdownNode? parent, string value) : base(parent, value)
    {
        
    }

    public override string ToHtml()
    {
        var textBuilder = new StringBuilder();
        foreach (var child in children)
        {
            textBuilder.Append(child.ToHtml());
        }

        return textBuilder.ToString();
    }
}