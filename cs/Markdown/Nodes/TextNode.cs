using System.Text;

namespace Markdown.Nodes;

public class TextNode : INode
{
    public string Text { get; }

    public TextNode(string text)
    {
        Text = text;
    }

    public void Render(StringBuilder sb)
    {
        sb.Append(Text);
    }
}