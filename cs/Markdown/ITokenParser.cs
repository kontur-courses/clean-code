using System.Collections.Generic;

namespace Markdown
{
    public interface ITokenParser
    {
        IEnumerable<string> GetTokens(string text);
    }
}