using System.Collections.Generic;
using Markdown.TokenParser.Parsers;

namespace Markdown.TokenParser
{
    public class TokenParser
    {
        public TokenTree[] Parse(IEnumerable<IToken> tokens)
        {
            var parser = new Parser(tokens);
            return parser.Parse();
        }
    }
}