using System.Text;
using Markdown.Nodes.Interfaces;

namespace Markdown.Nodes.Internal;

public class HeaderNode : InternalMarkdownNode
{
    public HeaderNode(string value) : base(value)
    {
    }

    public override string ToHtml()
    {
        var textBuilder = new StringBuilder();
        var controlCharacters = new StringBuilder();
        foreach (var child in Children)
            if (child.Value is "\n" or "\r")
                controlCharacters.Append(child.ToHtml());
            else
                textBuilder.Append(child.ToHtml());

        return $"<h1>{textBuilder}</h1>{controlCharacters}";
    }
}