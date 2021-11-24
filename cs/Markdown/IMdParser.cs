using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown
{
    public interface IMdParser
    {
        List<Token> ParseToTokens(string mdText);
    }
}
