using System.Text;

namespace Markdown.Parsing.Nodes;

public class ParagraphNode(List<InlineTypeNode> inlines) : BlockTypeNode
{
    public List<InlineTypeNode> Inlines { get; } = inlines;

    public override void RenderHtml(StringBuilder sb)
    {
        sb.Append("<p>");
        foreach (var inline in Inlines)
            inline.RenderHtml(sb);
        sb.Append("</p>");
    }
}