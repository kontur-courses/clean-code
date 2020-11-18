using System.Collections.Generic;

namespace Markdown
{
    public interface IParser
    {
        public List<TextToken> GetTextTokens(string text);
    }
}