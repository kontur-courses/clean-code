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
                RenderWrapped("h1", nodes, output);
                break;
            case BlockKind.Paragraph:
                RenderWrapped("p", nodes, output);
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
                output.Append(EscapeText(text.Text));
                break;
            case EmNode em:
                RenderWrapped("em", em.Children, output);
                break;
            case StrongNode strong:
                RenderWrapped("strong", strong.Children, output);
                break;
            case LinkNode link:
                RenderWrapped("a", link.Children, output, $" href=\"{EscapeAttribute(link.Url)}\"");
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(node), node, "Неизвестный узел дерева.");
        }
    }

    private static void RenderWrapped(
        string tag,
        IReadOnlyList<INode> children,
        StringBuilder output,
        string attributes = "")
    {
        output.Append('<').Append(tag).Append(attributes).Append('>');

        foreach (var child in children)
            RenderNode(child, output);

        output.Append("</").Append(tag).Append('>');
    }

    private static string EscapeText(string value)
    {
        var output = new StringBuilder(value.Length);

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
                default:
                    output.Append(symbol);
                    break;
            }

        return output.ToString();
    }

    private static string EscapeAttribute(string value)
    {
        return EscapeText(value).Replace("\"", "&quot;");
    }
}