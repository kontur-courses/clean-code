using System.Collections.Generic;
using Markdown.Engine.Parsers;

namespace Markdown
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