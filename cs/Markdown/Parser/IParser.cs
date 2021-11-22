using System.Collections.Generic;
using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown.Parser
{
    public interface IParser
    {
        List<IToken> GetTokens(string text);
    }
}