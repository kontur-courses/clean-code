using System;
using System.Collections.Generic;
using Markdown.Interfaces;

namespace Markdown
{
    public class TokenParser : ITokenParser
    {
        public IEnumerable<TokenNode> Parse(IEnumerable<Token> tokens) => throw new NotImplementedException();
    }
}