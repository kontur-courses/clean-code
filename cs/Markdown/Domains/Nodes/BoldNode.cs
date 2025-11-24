using System.Text;

namespace Markdown.Domains.Nodes;

public class BoldNode(List<Node>? children = null) : Node(children)
{
    public override void ConvertToHtml(StringBuilder sb)
    {
        sb.Append("<strong>");
        RenderChildren(sb);
        sb.Append("</strong>");
    }
}