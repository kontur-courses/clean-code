using Markdown.Exporter;
using Markdown.Lexer;
using Markdown.Parser;

namespace Markdown
{
    public class Md
    {
        public string Render(string markdown)
        {
            var tokens = new TokenSequence(new CharSequence(markdown));
            var tree = MarkdownParser.Parse(tokens);
            return tree.Export(new HtmlExporter());
        }
    }
}