using System.Collections.Generic;

namespace Markdown.Tokenization
{
    public interface ITokenReader
    {
        IEnumerable<Token> ReadAllTokens(string text);
    }
}