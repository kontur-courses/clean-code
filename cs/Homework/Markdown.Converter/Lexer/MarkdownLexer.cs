using System;
using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown.Lexer
{
    public class MarkdownLexer : ILexer
    {
        public IEnumerable<Token> Lex(string inputText)
        {
            var text = inputText ?? throw new ArgumentNullException(nameof(inputText));
            return new LexContext(text).Lex();
        }
    }
}