using MarkdownParser;

namespace Rendering.Html
{
    public class HtmlMarkdownConverter : IMarkdownConverter
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