using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown
{
    public interface IParser
    {
        public List<IToken> GetTextTokens(string text);
    }
}