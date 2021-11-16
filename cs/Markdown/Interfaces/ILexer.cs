using System.Collections.Generic;

namespace Markdown.Interfaces
{
    public interface ILexer
    {
        IEnumerable<Token> Lex(string text);
    }
}