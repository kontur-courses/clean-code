using MarkdownRenderer.Abstractions;
using MarkdownRenderer.Implementations.HtmlRenderers;
using MarkdownRenderer.Implementations.MarkdownParsers;

namespace MarkdownRenderer;

public class Md
{
    public string Render(string mdSource)
    {
        var converter = new DocumentConverter(
            new IElementParser[]
            {
                new MarkdownParagraphParser(),
                new MarkdownPlainTextParser(),
                new MarkdownItalicParser(),
                new MarkdownStrongParser()
            },
            new IElementRenderer[]
            {
                new HtmlParagraphRenderer(),
                new HtmlPlainTextRenderer(),
                new HtmlItalicRenderer(),
                new HtmlStrongRenderer()
            }
        );
        return converter.Convert(mdSource);
    }
}