using Markdown.Nodes.Interfaces;

namespace Markdown.Nodes.Leaf;

public class UrlNode : LeafMarkdownNode
{
    public UrlNode(string value) : base(value)
    {
    }

    public override string ToHtml()
    {
        return Value;
    }
}