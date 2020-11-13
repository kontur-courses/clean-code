using System.Collections.Generic;

namespace Markdown
{
    public interface IParser
    {
        public List<TokenMd> GetTokens(string text);
    }
}