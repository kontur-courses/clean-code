using System;
using System.Collections.Generic;
using Markdown.Interfaces;

namespace Markdown
{
    public class Lexer : ILexer
    {
        public IEnumerable<Token> Lex(string text)
        {
            if (text is null) throw new ArgumentNullException(nameof(text));
            var helper = new LexIterator(text);
            return helper.Lex();
        }
    }
}