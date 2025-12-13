using System.Text;
using Markdown.Nodes.Interfaces;

namespace Markdown.Nodes.Internal;

public class AltNode : InternalMarkdownNode
{
    public AltNode(string value) : base(value)
    {
    }

    public override string ToHtml()
    {
        var textBuilder = new StringBuilder();
        foreach (var child in Children)
            textBuilder.Append(child.ToHtml());

        return textBuilder.ToString();
    }
}