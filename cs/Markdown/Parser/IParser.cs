using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown.Parser
{
    public interface IParser
    {
        public IEnumerable<IToken> GetTokens(string textInMarkdown);
    }
}