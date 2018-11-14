using System.Collections.Generic;

namespace Markdown.TokenParser
{
    public interface ITokenParser
    {
        IEnumerable<string> GetTokens(string text);
    }
}