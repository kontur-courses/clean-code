using System.Collections.Generic;

namespace Markdown
{
    public interface ITextParser
    {
        IEnumerable<IToken> GetTokens(string text);
    }
}