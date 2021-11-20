using System;
using System.Collections.Generic;
using Markdown.Interfaces;

namespace Markdown
{
    public class TokenParser : ITokenParser
    {
        public IEnumerable<TokenNode> Parse(IEnumerable<Token> tokens)
        {
            if (tokens == null) throw new ArgumentNullException(nameof(tokens));
            using var enumerator = tokens.GetEnumerator();
            var iterator = new TokenParserIterator(enumerator);
            return iterator.Parse();
        }
    }
}