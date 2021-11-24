using Markdown.Lexer;
using Markdown.TagRenderer;
using Markdown.TokenParser;

namespace Markdown
{
    public class MarkdownConverter
    {
        private readonly ILexer lexer;
        private readonly ITokenParser parser;
        private readonly ITagRenderer renderer;

        public MarkdownConverter(ILexer lexer, ITokenParser parser, ITagRenderer renderer)
        {
            this.lexer = lexer;
            this.parser = parser;
            this.renderer = renderer;
        }

        public string Render(string mdText)
        {
            var tokens = lexer.Lex(mdText);
            var nodes = parser.Parse(tokens);
            var htmlText = renderer.Render(nodes);
            return htmlText;
        }
    }
}