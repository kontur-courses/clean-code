using System.Text;

namespace Markdown.Domains.Nodes;

public class ItalicNode(List<Node>? children = null) : Node(children)
{
    public override void ConvertToHtml(StringBuilder sb)
    {
        sb.Append("<em>");
        RenderChildren(sb);
        sb.Append("</em>");
    }
}