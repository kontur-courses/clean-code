using Markdown.Elements;
using Markdown.Parsers;

namespace Markdown
{
    public class Md
    {
        public string Render(string markdown)
        {
            var isEscapedCharAt = EscapesAnalyzer.GetBitMaskOfEscapedChars(markdown);
            var parser = new EmphasisParser(markdown, isEscapedCharAt, 0, RootElementType.Create());
            MarkdownElement rootElement = parser.Parse();
            var html = HtmlRenderer.RenderToHtml(rootElement);
            return EscapesAnalyzer.RemoveEscapeSlashes(html, new []{'_', '\\'});
        }
    }
}
