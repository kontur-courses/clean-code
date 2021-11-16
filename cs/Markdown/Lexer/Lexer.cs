using System;
using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown.Lexer
{
    public class Lexer : ILexer
    {
        private readonly string text;

        public Lexer(string text)
        {
            this.text = text;
        }
        
        public IEnumerable<Token> Lex()
        {
            throw new NotImplementedException();
        }
    }
}