using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class TokenDescription
    {
        public readonly TokenType tokenType;
        public readonly string marker;
        public readonly int length;

        public TokenDescription(TokenType tokenType, string tokenMarker, int tokenLength)
        {
            this.tokenType = tokenType;
            marker = tokenMarker;
            length = tokenLength;
        }

        public bool IsToken(string text, int position)
        {
            if (text.Length - position < marker.Length)
                return false;
            return text.Substring(position, marker.Length) == marker;
        }

        public Token ReadToken(string text, int position)
        {
            return new Token(text, position, tokenType, length);
        }
    }
}
