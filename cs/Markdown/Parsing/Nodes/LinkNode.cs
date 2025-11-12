using System.Text;
using Markdown.Rendering;

namespace Markdown.Parsing.Nodes;

public class LinkNode(List<InlineTypeNode> children, string url) : InlineTypeNode
{
    public List<InlineTypeNode> Children { get; } = children;
    public string Url { get; } = url;

    public override void RenderHtml(StringBuilder sb)
    {
        var href = HtmlRenderer.EscapeHtml(Url);
        sb.Append("<a href=\"").Append(href).Append("\">");
        foreach (var child in Children)
            child.RenderHtml(sb);
        sb.Append("</a>");
    }
}