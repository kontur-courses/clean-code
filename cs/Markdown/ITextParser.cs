using System.Collections.Generic;

namespace Markdown
{
    public interface ITextParser
    {
        IEnumerable<Token> GetTokens(string text);
    }
}