using MarkupConverter.AST.Blocks;
using MarkupConverter.AST.Inlines;
using System.Text;

namespace MarkupConverter.Renderers;

public class HtmlRenderer : IRenderer
{
    public string Render(Document document)
    {
        var htmlBlocks = document.Blocks.Select(Render);
        return string.Join(Environment.NewLine, htmlBlocks);
    }

    private string Render(Block block)
    {
        return block switch
        {
            Header header => RenderHeader(header),
            Paragraph paragraph => RenderParagraph(paragraph),
            _ => throw new NotSupportedException($"Block type {block.GetType().Name} not supported")
        };
    }

    private string RenderHeader(Header header)
    {
        var tag = $"h{header.Level}";
        var content = RenderInlineList(header.Inlines);
        return $"<{tag}>{content}</{tag}>";
    }

    private string RenderParagraph(Paragraph paragraph)
    {
        var content = RenderInlineList(paragraph.Inlines);
        return $"<p>{content}</p>";
    }

    private string RenderInlineList(List<Inline> inlines)
    {
        var builder = new StringBuilder();

        foreach (var inline in inlines) builder.Append(RenderInline(inline));

        return builder.ToString();
    }

    private string RenderInline(Inline inline)
    {
        return inline switch
        {
            Text text => RenderText(text),
            Bold bold => RenderBold(bold),
            Italic italic => RenderItalic(italic),
            _ => throw new NotSupportedException($"Inline type {inline.GetType().Name} not supported")
        };
    }

    private string RenderText(Text text)
    {
        return System.Net.WebUtility.HtmlEncode(text.Content);
    }

    private string RenderBold(Bold bold)
    {
        var content = RenderInlineList(bold.Inlines);
        return $"<strong>{content}</strong>";
    }

    private string RenderItalic(Italic italic)
    {
        var content = RenderInlineList(italic.InlineLeaves);
        return $"<em>{content}</em>";
    }
}