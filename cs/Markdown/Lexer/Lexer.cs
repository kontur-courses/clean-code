using System;
using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown.Lexer
{
    public class Lexer : ILexer
    {
        public IEnumerable<Token> Lex(string text)
        {
            if (text is null) throw new ArgumentNullException(nameof(text));
            var lexIterator = new LexIterator(text);
            return lexIterator.Lex();
        }
    }
}