using Markdown.Elements;
using Markdown.Parsers;
using Markdown.Analyzers;

namespace Markdown
{
    public class Md
    {
        public string Render(string markdown)
        {
            var syntaxAnalysisResult = SyntaxAnalyzer.AnalyzeSyntax(markdown);
            var parser = new EmphasisParser(syntaxAnalysisResult, 0, RootElementType.Create());
            MarkdownElement rootElement = parser.Parse();
            var html = HtmlRenderer.RenderToHtml(rootElement);
            return EscapesAnalyzer.RemoveEscapeSlashes(html, new []{'_', '\\'});
        }
    }
}
