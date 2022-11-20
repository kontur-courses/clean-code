using Markdown.Parsers;
using Markdown.Renderers;

namespace Markdown
{
    public class Md
    {
        private readonly IRenderer renderer;
        public Md(IRenderer renderer)
        {
            this.renderer = renderer;
        }

        public string Render(string markdownText)
        {
            if (string.IsNullOrEmpty(markdownText))
                return markdownText;

            var markdownParser = new MarkdownParser(markdownText);
            var parsedDocument = markdownParser.GetParsedDocument();
            return renderer.Render(parsedDocument);
        }
    }
}