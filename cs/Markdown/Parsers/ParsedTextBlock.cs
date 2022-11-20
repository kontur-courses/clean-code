using System.Collections.Generic;
using Markdown.Parsers.Tokens;

namespace Markdown.Parsers
{
    public class ParsedTextBlock
    {
        public IEnumerable<IToken> Tokens { get; }

        public ParsedTextBlock(IEnumerable<IToken> tokens)
        {
            Tokens = tokens;
        }
    }
}
