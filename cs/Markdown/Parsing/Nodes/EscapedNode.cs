using System.Text;
using Markdown.Rendering;

namespace Markdown.Parsing.Nodes;

public class EscapedNode(char escapedChar) : InlineTypeNode
{
    public char EscapedChar { get; } = escapedChar;

    public override void RenderHtml(StringBuilder sb)
    {
        sb.Append(HtmlRenderer.EscapeHtml(EscapedChar.ToString()));
    }
}