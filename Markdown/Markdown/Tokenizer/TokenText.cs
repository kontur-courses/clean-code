using System.Collections.Generic;

namespace Markdown
{
    public class TokenText
    {
        public readonly string Source;
        public readonly IEnumerable<IToken> InTextTokens;

        public TokenText(string source, IEnumerable<IToken> inTextTokens)
        {
            Source = source;
            InTextTokens = inTextTokens;
        }
    }
}