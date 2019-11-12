using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class TokenDescription
    {
        public readonly TokenType TokenType;
        public readonly string TokenMarker;
        private readonly Func<string, int, int> readToken;

        public TokenDescription(TokenType tokenType, string tokenMarker, 
            Func<string, int, int> readToken)
        {
            TokenType = tokenType;
            TokenMarker = tokenMarker;
            this.readToken = readToken;
        }

        public bool TryReadToken(string text, int position, out Token token)
        {
            token = null;
            if (position + TokenMarker.Length > text.Length)
                return false;
            var length = readToken(text, position);
            if (length == 0)
                return false;
            token = new Token(text, position, TokenType, length);
            return true;
        }
    }
}
