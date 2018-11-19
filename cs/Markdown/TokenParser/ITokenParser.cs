using System.Collections.Generic;
using Markdown.Data;

namespace Markdown.TokenParser
{
    public interface ITokenParser
    {
        IEnumerable<Token> GetTokens(string text);
    }
}