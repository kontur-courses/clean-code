using System.Text;

namespace Markdown.Domains.Nodes;

public class TextNode(string text) : Node
{
    private string Text { get; } = text;

    public override void ConvertToHtml(StringBuilder sb)
    {
        sb.Append(Text);
    }
}