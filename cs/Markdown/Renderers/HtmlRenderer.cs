using System.Text;
using Markdown.Entities;
using Markdown.Enums;
using Markdown.Parsers;

namespace Markdown.Renderers;

public sealed class HtmlRenderer(
    InlineParser inlineParser)
{
    public string Render(IEnumerable<MarkdownBlock> markdownBlocks)
    {
        var stringBuilder = new StringBuilder();
        foreach (var markdownBlock in markdownBlocks)
        {
            var renderedBlock = RenderMarkdownBlock(markdownBlock);
            stringBuilder.Append(renderedBlock);
        }
        
        return stringBuilder.ToString();
    }

    private string RenderMarkdownBlock(MarkdownBlock markdownBlock)
    {
        switch (markdownBlock.Type)
        {
            case MarkdownBlockType.Heading:
                return RenderHeading(markdownBlock);
            case MarkdownBlockType.Paragraph:
                return RenderParagraph(markdownBlock);
            default:
                throw new NotImplementedException();
        }
    }

    private string RenderHeading(MarkdownBlock markdownBlock)
    {
        throw new NotImplementedException();
    }
    
    private string RenderParagraph(MarkdownBlock markdownBlock)
    {
        throw new NotImplementedException();
    }
}