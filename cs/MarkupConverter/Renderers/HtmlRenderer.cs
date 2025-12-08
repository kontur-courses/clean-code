using MarkupConverter.AST.Blocks;
using MarkupConverter.AST.Inlines;

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
        var content = RenderInline(header.Inlines);
        return $"<{tag}>{content}</{tag}>";
    }

    private string RenderParagraph(Paragraph paragraph)
    {
        var content = RenderInline(paragraph.Inlines);
        return $"<p>{content}</p>";
    }


    private string RenderInline(List<Inline> inlineContent)
    {
        var res = string.Empty;
        foreach (var variabInline in inlineContent)
            if (variabInline is Text text)
                res = text.Content;

        return res;
    }
}