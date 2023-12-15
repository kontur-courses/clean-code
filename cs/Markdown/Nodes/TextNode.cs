namespace Markdown;

public class TextNode : SimpleTag
{
    public TextNode(string text) : base(NodeType.TextNode, text)
    {
    }
}