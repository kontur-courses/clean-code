using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Tokens
{
    public class Token
    {
        public TokenType Type { get; }

        public int Start { get; }
        public int End { get; }

        public int Length => End - Start + 1;

        public Token(TokenType type, int start, int end)
        {
            Type = type;
            Start = start;
            End = end;
        }
    }
}
