using Markdown.Nodes.Interfaces;

namespace Markdown.Nodes.Leaf;

public class TextNode : LeafMarkdownNode
{
    public TextNode(string value) : base(value)
    {
    }

    public override string ToHtml()
    {
        return Value;
    }
}