using System.Text;
using Markdown.Syntax;

namespace Markdown.Rendering;

internal static class HtmlRenderer
{
    public static string Render(DocumentNode document)
    {
        ArgumentNullException.ThrowIfNull(document);

        var output = new StringBuilder();

        for (var i = 0; i < document.Blocks.Count; i++)
        {
            if (i > 0)
                output.Append('\n');

            RenderBlock(document.Blocks[i], output);
        }

        return output.ToString();
    }

    private static void RenderBlock(BlockNode node, StringBuilder output)
    {
        switch (node)
        {
            case ParagraphNode paragraph:
                RenderInlineContainer("p", paragraph.Children, output);
                break;

            case HeadingNode heading:
                RenderInlineContainer("h1", heading.Children, output);
                break;

            case UnorderedListNode unorderedList:
                RenderUnorderedList(unorderedList, output);
                break;

            default:
                throw new NotSupportedException(
                    $"Block type '{node.GetType().Name}' is not supported yet.");
        }
    }

    private static void RenderUnorderedList(
        UnorderedListNode unorderedList,
        StringBuilder output)
    {
        output.Append("<ul>");

        foreach (var item in unorderedList.Items)
        {
            RenderInlineContainer("li", item.Children, output);
        }

        output.Append("</ul>");
    }

    private static void RenderInline(InlineNode node, StringBuilder output)
    {
        switch (node)
        {
            case TextNode text:
                AppendEscapedText(text.Text, output);
                break;

            case EmphasisNode emphasis:
                RenderInlineContainer("em", emphasis.Children, output);
                break;

            case StrongNode strong:
                RenderInlineContainer("strong", strong.Children, output);
                break;

            default:
                throw new NotSupportedException(
                    $"Inline type '{node.GetType().Name}' is not supported yet.");
        }
    }

    // Общая структура p, h1, li, em и strong это:
    // <открывающий тег> дочерние инлайн-узлы </закрывающий тег>
    private static void RenderInlineContainer(
        string tag,
        IReadOnlyList<InlineNode> children,
        StringBuilder output)
    {
        output.Append('<').Append(tag).Append('>');

        foreach (var child in children)
        {
            RenderInline(child, output);
        }

        output.Append("</").Append(tag).Append('>');
    }

    private static void AppendEscapedText(string text, StringBuilder output)
    {
        var unescapedStart = 0;

        for (var i = 0; i < text.Length; i++)
        {
            var escaped = GetHtmlEscape(text[i]);

            if (escaped is null)
                continue;

            output
                .Append(text, unescapedStart, i - unescapedStart)
                .Append(escaped);

            unescapedStart = i + 1;
        }

        output.Append(text, unescapedStart, text.Length - unescapedStart);
    }

    private static string? GetHtmlEscape(char character)
    {
        return character switch
        {
            '&' => "&amp;",
            '<' => "&lt;",
            '>' => "&gt;",
            _ => null
        };
    }
}
