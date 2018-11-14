using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    internal class HtmlCreator
    {
        public string CreateFromTokens(IEnumerable<Token> tokens)
        {
            return string.Join( "", tokens.Select(token => token.ToHtml()));
        }

    }
}