using System.Text;
using Markdown.Rendering;

namespace Markdown.Parsing.Nodes;

public class TextNode(string text) : InlineTypeNode
{
    public string Text { get; } = text;

    public override void RenderHtml(StringBuilder sb)
    {
        sb.Append(HtmlRenderer.EscapeHtml(Text));
    }
}