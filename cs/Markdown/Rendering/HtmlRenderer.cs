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
            sb.Append(node.ToHtmlString());
        }

        return sb.ToString();
    }
}