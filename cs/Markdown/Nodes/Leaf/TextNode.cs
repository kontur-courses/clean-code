using Markdown.Nodes.Interfaces;

namespace Markdown.Nodes.Leaf;

public class TextNode : LeafMarkdownNode
{
    public override string ToHtml()
    {
        throw new NotImplementedException();
    }

    public TextNode(MarkdownNode? parent, string value) : base(parent, value)
    {
    }
}