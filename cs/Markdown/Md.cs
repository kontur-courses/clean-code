using Markdown.Exporter;
using Markdown.Lexer;
using Markdown.Parser;

namespace Markdown
{
    public class Md
    {
        public string Render(string markdown)
        {
            var tokenizer = new Tokenizer(markdown);
            var tree = MarkdownParser.Parse(tokenizer);
            return tree.Export(new HtmlExporter());
        }
    }
}