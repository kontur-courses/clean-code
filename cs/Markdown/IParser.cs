using System.Collections.Generic;

namespace Markdown
{
    public interface IParser
    {
        public List<IToken> GetTextTokens(string text);
    }
}