using System;
using System.Collections.Generic;
using Markdown.Interfaces;

namespace Markdown
{
    public class Lexer : ILexer
    {
        public IEnumerable<Token> Lex(string text) => throw new NotImplementedException();
    }
}