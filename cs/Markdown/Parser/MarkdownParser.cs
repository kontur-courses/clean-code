using System;
using System.Collections.Generic;
using Markdown.Lexer;
using Markdown.Tokens;

namespace Markdown.Parser
{
    public class MarkdownParser : IParser
    {
        private readonly ILexer lexer;

        public MarkdownParser(ILexer lexer)
        {
            this.lexer = lexer;
        }

        public IEnumerable<Token> Parse(string text)
        {
            throw new NotImplementedException();
        }
    }
}