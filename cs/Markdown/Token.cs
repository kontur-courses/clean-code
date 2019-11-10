using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class Token
    {
        public TokenType tokenType;

        public readonly int position;
        public int length;
        public readonly string text;

        public Token(string text, 
            int position,
            TokenType tokenType = TokenType.Raw,
            int length = 0)
        {
            this.tokenType = tokenType;
            this.text = text;
            this.position = position;
        }
    }
}
