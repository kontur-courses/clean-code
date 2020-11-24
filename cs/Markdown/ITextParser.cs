using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown
{
    public interface ITextParser
    {
        IEnumerable<Token> GetTokens(string text);
    }
}