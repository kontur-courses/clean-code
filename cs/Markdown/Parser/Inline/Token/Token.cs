using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Parser.Inline.Token
{
    public class Token
    {
        public TokenType Type { get; private set; }
        public string Value { get; private set;  }
        public int Position { get; private set; }
        public bool IsClose { get; private set; }

        public Token(TokenType type, string value, int position, bool isClose = false)
        {
            Type = type;
            Value = value;
            Position = position;
            IsClose = isClose;
        }

        public void Close()
        {
            IsClose = true;
        }
    }
}
