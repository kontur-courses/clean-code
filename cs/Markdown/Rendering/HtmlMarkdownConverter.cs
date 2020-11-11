using MarkdownParser;
using Rendering.Html;

namespace Rendering
{
    public class HtmlMarkdownConverter
    {
        private readonly MarkdownDocumentParser parser;
        private readonly HtmlRenderer renderer;

        public HtmlMarkdownConverter(MarkdownDocumentParser parser, HtmlRenderer renderer)
        {
            this.parser = parser;
            this.renderer = renderer;
        }

        public string Convert(string markdown)
        {
            var parsedDoc = parser.Parse(markdown);
            return renderer.Render(parsedDoc);
        }
    }
}