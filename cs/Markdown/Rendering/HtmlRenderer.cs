using System.Net;
using System.Text;
using Markdown.Domain;

namespace Markdown.Rendering;

public static class HtmlRenderer
{
    public static string RenderNodes(IReadOnlyList<Node> nodes)
    {
        var sb = new StringBuilder();

        foreach (var node in nodes)
        {
            RenderNode(node, sb);
        }

        return sb.ToString();
    }

    private static void RenderNode(Node node, StringBuilder sb)
    {
        switch (node)
        {
            case TextNode text:
                sb.Append(WebUtility.HtmlEncode(text.Text));
                break;

            case HeaderNode header:
                RenderContainer(header, "h1", sb);
                break;

            case ParagraphNode paragraph:
                RenderContainer(paragraph, "p", sb);
                break;

            case EmphasisNode emphasis:
                RenderContainer(emphasis, "em", sb);
                break;

            case StrongNode strong:
                RenderContainer(strong, "strong", sb);
                break;

            default:
                throw new NotSupportedException(
                    $"Unsupported node type: {node.GetType().Name}");
        }
    }

    private static void RenderContainer(
        ContainerNode node,
        string tag,
        StringBuilder sb)
    {
        sb.Append('<').Append(tag).Append('>');

        foreach (var child in node.Children)
        {
            RenderNode(child, sb);
        }

        sb.Append("</").Append(tag).Append('>');
    }
}