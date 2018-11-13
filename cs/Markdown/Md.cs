using Markdown.Elements;
using Markdown.Parsers;

namespace Markdown
{
    public class Md
    {
        public string Render(string markdown)
        {
            var parser = new EmphasisParser(markdown, 0, RootElementType.Create());
            MarkdownElement rootElement = parser.Parse();
            return HtmlRenderer.RenderToHtml(rootElement);
        }
    }
}
