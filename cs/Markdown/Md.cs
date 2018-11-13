using Markdown.Elements;
using Markdown.Parsers;

namespace Markdown
{
    public class Md
    {
        public string Render(string markdown)
        {
            MarkdownElement rootElement = EmphasisParser.ParseElement(markdown, 0, RootElementType.Create());
            return HtmlRenderer.RenderToHtml(rootElement);
        }
    }
}
