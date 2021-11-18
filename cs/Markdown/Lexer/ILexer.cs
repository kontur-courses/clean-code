using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Markdown.Tokens;

namespace Markdown.Lexer
{
    public interface ILexer
    {
        IEnumerable<Token> Lex([NotNull] string text);
    }
}