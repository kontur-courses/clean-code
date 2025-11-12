using System.Text;

namespace Markdown.Parsing.Nodes;

public class HeaderNode(List<InlineTypeNode> inlines) : BlockTypeNode
{
    public List<InlineTypeNode> Inlines { get; } = inlines;

    public override void RenderHtml(StringBuilder sb)
    {
        sb.Append("<h1>");
        foreach (var inline in Inlines)
            inline.RenderHtml(sb);
        sb.Append("</h1>");
    }
}