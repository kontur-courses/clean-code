using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    class Token
    {
        public readonly int StartIndex;
        public readonly string Text;
        public readonly TokenType Type;


        public Token(int startIndex, string text, TokenType type)
        {
            StartIndex = startIndex;
            Text = text;
            Type = type;
        }
    }
}
