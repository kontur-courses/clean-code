using System.Collections.Generic;

namespace Markdown
{
    public interface IMarkdownParser
    {
        public List<TokenMd> GetTokens(string text);
    }
}