namespace Markdown.Nodes;

public class TextNode : SimpleNode
{
    public TextNode(string text) : base(text)
    {
    }

    public override string ToString() => Text;
}