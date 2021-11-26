using System.Collections.Generic;
using Markdown.Interfaces;

namespace Markdown
{
    public class TokenParser : ITokenParser
    {
        public TokenTree[] Parse(IEnumerable<IToken> tokens)
        {
            var parser = new Parser(tokens);
            return parser.Parse();
        }
    }
}