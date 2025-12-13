using System.Text;
using Markdown.Nodes.Interfaces;

namespace Markdown.Nodes.Internal;

public class ItalicNode : InternalMarkdownNode
{
    public ItalicNode(string value) : base(value)
    {
    }

    public override string ToHtml()
    {
        var textBuilder = new StringBuilder();
        foreach (var child in Children) textBuilder.Append(child.ToHtml());

        return $"<em>{textBuilder}</em>";
    }
}