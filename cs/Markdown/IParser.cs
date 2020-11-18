using System.Collections.Generic;

namespace Markdown
{
    public interface IParser
    {
        List<TextToken> GetTextTokens(string text);
    }
}