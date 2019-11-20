using System;

namespace Markdown
{
    public class TokenDescription
    {
        public readonly TokenType TokenType;
        private readonly Func<string, int, int> readToken;

        public TokenDescription(TokenType tokenType, 
            Func<string, int, int> readToken)
        {
            TokenType = tokenType;
            this.readToken = readToken;
        }

        public bool TryReadToken(string text, int position, out Token token)
        {
            token = null;
            var length = readToken(text, position);
            if (length == 0)
                return false;
            token = new Token(text, position, TokenType, length);
            return true;
        }
    }
}
