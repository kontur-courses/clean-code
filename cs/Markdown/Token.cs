using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class Token
    {
        public TokenType Type { get; }
        public string Value { get; }
        public int StartIndex { get; }
        public Token Previous { get; }
        public Token Next { get; set; }
        public bool IgnoreAsTag { get; private set; }

        public Token(TokenType type, string value, int startIndex, Token previous)
        {
            Previous = previous;
            Type = type;
            Value = value;
            StartIndex = startIndex;
        }

        public Token IgnoreTag()
        {
            IgnoreAsTag = true;
            return this;
        }
    }
}
