using System;
using System.Collections.Generic;
using Markdown.Lexer;
using Markdown.SyntaxParser;
using Markdown.Tokens;

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

        public IEnumerable<Token> Parse(string text)
        {
            var lexedTokens = lexer.Lex(text);
            return syntaxParser.Parse(lexedTokens);
        }
    }
}