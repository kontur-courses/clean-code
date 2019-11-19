using System.Collections.Generic;

namespace Markdown.Tokens
{
    public interface ITokenReader
    {
        IEnumerable<Token> ReadAllTokens(string text);
    }
}