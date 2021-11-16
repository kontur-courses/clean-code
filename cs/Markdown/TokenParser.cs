using System;
using System.Collections.Generic;
using Markdown.Interfaces;

namespace Markdown
{
    public class TokenParser : ITokenParser
    {
        public IEnumerable<Token> Parse(IEnumerable<Token> tokens) => throw new NotImplementedException();
    }
}