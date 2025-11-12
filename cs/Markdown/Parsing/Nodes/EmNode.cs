using System.Text;

namespace Markdown.Parsing.Nodes;

public class EmNode(List<InlineTypeNode> children) : InlineTypeNode
{
    public List<InlineTypeNode> Children { get; } = children;

    public override void RenderHtml(StringBuilder sb)
    {
        sb.Append("<em>");
        foreach (var child in Children)
            child.RenderHtml(sb);
        sb.Append("</em>");
    }
}