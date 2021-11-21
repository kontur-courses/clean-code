using System.Collections.Generic;
using Markdown.Lexer;
using Markdown.SyntaxParser;

namespace Markdown.Parser
{
    public class MarkdownParser : IParser
    {
        private readonly ILexer lexer;
        private readonly ISyntaxParser syntaxParser;

        public MarkdownParser(ILexer lexer, ISyntaxParser syntaxParser)
        {
            this.lexer = lexer;
            this.syntaxParser = syntaxParser;
        }

        public IEnumerable<TokenTree> Parse(string text)
        {
            var lexedTokens = lexer.Lex(text);
            return syntaxParser.Parse(lexedTokens);
        }
    }
}