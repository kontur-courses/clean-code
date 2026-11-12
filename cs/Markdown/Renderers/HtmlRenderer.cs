using System.Text;
using Markdown.Entities;
using Markdown.Parsers;

namespace Markdown.Renderers;

public sealed class HtmlRenderer(InlineParser inlineParser)
{
    public string Render(IEnumerable<MarkdownBlock> markdownBlocks)
    {
        var stringBuilder = new StringBuilder();
        foreach (var markdownBlock in markdownBlocks)
        {
            if (stringBuilder.Length > 0)
            {
                stringBuilder.Append('\n');
            }

            var renderedBlock = RenderMarkdownBlock(markdownBlock);
            stringBuilder.Append(renderedBlock);
        }

        return stringBuilder.ToString();
    }

    private string RenderMarkdownBlock(MarkdownBlock markdownBlock)
    {
        return markdownBlock.Tag.Render(markdownBlock.Content, inlineParser);
    }
}