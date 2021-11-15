using System;
using System.Collections.Generic;
using Markdown.Models;

namespace Markdown
{
    public class TokenReader
    {
        private string text;
        private IToken[] tokens;

        public TokenReader(string text, params IToken[] tokens)
        {
            this.tokens = tokens;
            this.text = text;
        }

        public IEnumerable<TokenQueryMatch> FindAll() => throw new NotImplementedException();
    }
}