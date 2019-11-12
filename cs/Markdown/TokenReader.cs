using System;
using System.Collections.Generic;

namespace Markdown
{
    public class TokenReader
    {
        private readonly string text;

        public TokenReader(string text)
        {
            this.text = text;
        }

        public IEnumerable<Token> ReadTokens(Func<string, int, bool> isSeparator,
            Func<string, int, string> getSeparator)
        {
            throw new NotImplementedException();
        }
    }
}