using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Markdown;

public interface IRenderer
{
    string Render(IEnumerable<Node> nodes);
}

public class HtmlRenderer : IRenderer
{
    public string Render(IEnumerable<Node>? nodes)
    {
        if (nodes == null) return string.Empty;

        var sb = new StringBuilder();

        foreach (var node in nodes)
        {
            RenderNode(node, sb);
        }

        return sb.ToString();
    }

    private static void RenderNode(Node? node, StringBuilder sb)
    {
        if (node == null) return;

        switch (node.Type)
        {
            case NodeType.Header:
                sb.Append("<h1>");
                foreach (var child in node.Children)
                    RenderNode(child, sb);
                sb.Append("</h1>");
                break;

            case NodeType.Strong:
                sb.Append("<strong>");
                foreach (var child in node.Children)
                    RenderNode(child, sb);
                sb.Append("</strong>");
                break;

            case NodeType.Emphasis:
                sb.Append("<em>");
                if (node.Children != null)
                    foreach (var child in node.Children)
                        RenderNode(child, sb);
                else
                    sb.Append(node.Value);
                sb.Append("</em>");
                break;
            
            case NodeType.Link:
                var parts = node.Value?.Split('|');
                if (parts == null || parts.Length < 2) 
                    return;

                var linkText = WebUtility.HtmlEncode(parts[0]);
                var url = WebUtility.HtmlEncode(parts[1]);
                var title = (parts.Length > 2 && !string.IsNullOrWhiteSpace(parts[2]))
                    ? $" title=\"{WebUtility.HtmlEncode(parts[2])}\""
                    : "";

                sb.Append($"<a href=\"{url}\"{title}>{linkText}</a>");
                break;


            case NodeType.Text:
            default:
                var text = WebUtility.HtmlEncode(node.Value);
                sb.Append(text);
                if (node.Children != null && node.Children.Count > 0)
                    RenderInlineChildren(node.Children, sb);
                break;
        }
    }

    private static void RenderInlineChildren(List<Node>? children, StringBuilder sb)
    {
        if (children == null || children.Count == 0) return;

        foreach (var child in children)
            RenderNode(child, sb);
    }
}