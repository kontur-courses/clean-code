using System.Collections.Generic;

namespace Markdown
{
    public interface ITextParser
    {
        List<Token> GetTokens(string text, string context);
    }
}