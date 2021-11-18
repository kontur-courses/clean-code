using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown.Lexer
{
    public interface ILexer
    {
        IEnumerable<Token> Lex(string inputText);
    }
}