using Markdown.Exporter;
using Markdown.Lexer;
using Markdown.Parser;

namespace Markdown
{
    public class Md
    {
        public static string Render(string markdown)
        {
            var tokenizer = new Tokenizer();
            var tree = MarkdownParser.Parse(tokenizer.GetTokens(markdown));
            return tree.Export(new HtmlExporter());
        }
    }
}