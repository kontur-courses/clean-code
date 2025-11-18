using System.Net;
using System.Text;
using Markdown.Parsing;

namespace Markdown;

public class HtmlRender
{
    public string Render(DocumentNode document)
    {
        var sb = new StringBuilder();
        RenderNodes(document.Children, sb);
        return sb.ToString();
    }

    private static void RenderNodes(IEnumerable<INode> nodes, StringBuilder sb)
    {
        foreach (var node in nodes)
            node.Render(sb);
    }
}