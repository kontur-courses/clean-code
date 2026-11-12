using System.Text;
using Markdown.Blocks;
using Markdown.Tree;

namespace Markdown.Rendering;

public class HtmlRenderer
{
    public string Render(BlockKind kind, IReadOnlyList<INode> nodes)
    {
        var output = new StringBuilder();

        RenderContainer(TagFor(kind), nodes, output);

        return output.ToString();
    }

    private static string TagFor(BlockKind kind)
    {
        return kind switch
        {
            BlockKind.Header => HtmlTags.Header,
            BlockKind.Paragraph => HtmlTags.Paragraph,
            _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, "Неизвестный вид блока.")
        };
    }

    private static void RenderNode(INode node, StringBuilder output)
    {
        switch (node)
        {
            case TextNode text:
                AppendEscaped(output, text.Text);
                break;
            case EmNode em:
                RenderContainer(HtmlTags.Emphasis, em.Children, output);
                break;
            case StrongNode strong:
                RenderContainer(HtmlTags.Strong, strong.Children, output);
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
        output.Append(HtmlTags.TagStart).Append(tag).Append(HtmlTags.TagEnd);
        RenderChildren(children, output);
        output.Append(HtmlTags.ClosingTagPrefix).Append(tag).Append(HtmlTags.TagEnd);
    }

    private static void RenderLink(LinkNode link, StringBuilder output)
    {
        output.Append(HtmlTags.LinkOpen);
        AppendEscaped(output, link.Url, escapeQuotes: true);
        output.Append(HtmlTags.LinkOpenEnd);
        RenderChildren(link.Children, output);
        output.Append(HtmlTags.LinkClose);
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
                    output.Append(HtmlTags.Ampersand);
                    break;
                case '<':
                    output.Append(HtmlTags.LessThan);
                    break;
                case '>':
                    output.Append(HtmlTags.GreaterThan);
                    break;
                case '"' when escapeQuotes:
                    output.Append(HtmlTags.Quote);
                    break;
                default:
                    output.Append(symbol);
                    break;
            }
    }
}
