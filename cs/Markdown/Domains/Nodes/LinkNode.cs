using System.Text;

namespace Markdown.Domains.Nodes;

public class LinkNode(LinkNodeType type, List<Node>? children = null) : Node(children)
{
    private LinkNodeType LinkNodeType { get; } = type;

    public override void ConvertToHtml(StringBuilder sb)
    {
        if (LinkNodeType != LinkNodeType.LinkRoot || Children.Count < 2)
        {
            RenderChildren(sb);
            return;
        }

        var textNode = Children
            .OfType<LinkNode>()
            .FirstOrDefault(n => n.LinkNodeType == LinkNodeType.MeaningText);

        var urlNode = Children
            .OfType<LinkNode>()
            .FirstOrDefault(n => n.LinkNodeType == LinkNodeType.LinkText);

        if (textNode == null || urlNode == null)
        {
            RenderChildren(sb);
            return;
        }

        var textBuilder = new StringBuilder();
        textNode.ConvertToHtml(textBuilder);

        var urlBuilder = new StringBuilder();
        urlNode.ConvertToHtml(urlBuilder);

        sb.Append("<a href=\"");
        sb.Append(urlBuilder);
        sb.Append("\">");
        sb.Append(textBuilder);
        sb.Append("</a>");
    }
}