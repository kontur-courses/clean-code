using System.Text;

namespace Markdown.Parsing.Nodes;

public class StrongNode(List<InlineTypeNode> children) : InlineTypeNode
{
    public List<InlineTypeNode> Children { get; } = children;

    public override void RenderHtml(StringBuilder sb)
    {
        sb.Append("<strong>");
        foreach (var child in Children)
            child.RenderHtml(sb);
        sb.Append("</strong>");
    }
}