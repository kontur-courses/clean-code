using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class Token
    {
        public readonly TokenType TokenType;
        public readonly int Position;
        public readonly int Length;
        public readonly string Text;

        public Token(string text, 
            int position,
            TokenType tokenType = TokenType.Text,
            int length = 0)
        {
            TokenType = tokenType;
            Position = position;
            Length = length;
            Text = text.Substring(Position, Length);
        }

        public string GetTokenString()
        {
            return Text;
        }
    }
}
