using MarkdownRenderer.Abstractions;
using MarkdownRenderer.Abstractions.ElementsParsers;
using MarkdownRenderer.Implementations;
using MarkdownRenderer.Implementations.HtmlRenderers;
using MarkdownRenderer.Implementations.MarkdownParsers;
using MarkdownRenderer.Implementations.MarkdownParsers.SpecialInlineParsers;
using MarkdownRenderer.Implementations.MarkdownParsers.SpecialLineParsers;
using IElementRenderer = MarkdownRenderer.Abstractions.ElementsRenderers.IElementRenderer;

namespace MarkdownRenderer;

public class Md
{
    public string Render(string mdSource) =>
        CreateConverter().Convert(mdSource);

    private IDocumentsConverter CreateConverter() =>
        new ParallelDocumentConverter(
            new DefaultLineParser(new IElementParser[]
                {
                    new MarkdownEscapeSequenceParser(),
                    new MarkdownParagraphParser(),
                    new MarkdownHeaderParser(),
                    new MarkdownPlainTextParser(),
                    new MarkdownItalicParser(),
                    new MarkdownStrongParser(),
                    new MarkdownSimpleLinkParser(),
                    new MarkdownTitledLinkParser()
                }
            ),
            new DefaultLineRenderer(new IElementRenderer[]
                {
                    new HtmlEscapeSequenceRenderer(),
                    new HtmlParagraphRenderer(),
                    new HtmlHeaderRenderer(),
                    new HtmlPlainTextRenderer(),
                    new HtmlItalicRenderer(),
                    new HtmlStrongRenderer(),
                    new HtmlLinkRenderer()
                }
            )
        );
}