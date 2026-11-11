using System.Text;
using Markdown.Blocks;
using Markdown.Tree;

namespace Markdown.Rendering;

public class HtmlRenderer
{
    public string Render(BlockKind kind, IReadOnlyList<INode> nodes)
    {
        var output = new StringBuilder();

        switch (kind)
        {
            case BlockKind.Header:
                RenderContainer("h1", nodes, output);
                break;
            case BlockKind.Paragraph:
                RenderContainer("p", nodes, output);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(kind), kind, "Неизвестный вид блока.");
        }

        return output.ToString();
    }

    private static void RenderNode(INode node, StringBuilder output)
    {
        switch (node)
        {
            case TextNode text:
                AppendEscaped(output, text.Text);
                break;
            case EmNode em:
                RenderContainer("em", em.Children, output);
                break;
            case StrongNode strong:
                RenderContainer("strong", strong.Children, output);
                break;
            case LinkNode link:
                RenderLink(link, output);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(node), node, "Неизвестный узел дерева.");
        }
    }

    private static void RenderContainer(string tag, IReadOnlyList<INode> children, StringBuilder output)
    {
        output.Append('<').Append(tag).Append('>');
        RenderChildren(children, output);
        output.Append("</").Append(tag).Append('>');
    }

    private static void RenderLink(LinkNode link, StringBuilder output)
    {
        output.Append("<a href=\"");
        AppendEscaped(output, link.Url, escapeQuotes: true);
        output.Append("\">");
        RenderChildren(link.Children, output);
        output.Append("</a>");
    }

    private static void RenderChildren(IReadOnlyList<INode> children, StringBuilder output)
    {
        foreach (var child in children)
            RenderNode(child, output);
    }

    private static void AppendEscaped(StringBuilder output, string value, bool escapeQuotes = false)
    {
        foreach (var symbol in value)
            switch (symbol)
            {
                case '&':
                    output.Append("&amp;");
                    break;
                case '<':
                    output.Append("&lt;");
                    break;
                case '>':
                    output.Append("&gt;");
                    break;
                case '"' when escapeQuotes:
                    output.Append("&quot;");
                    break;
                default:
                    output.Append(symbol);
                    break;
            }
    }
}