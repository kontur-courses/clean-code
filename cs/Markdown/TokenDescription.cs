using System;

namespace Markdown
{
    public class TokenDescription
    {
        public Func<string, int, Token> ReadToken;

        public TokenDescription(Func<string, int, Token> readToken)
        {
            ReadToken = readToken;
        }
    }
}
