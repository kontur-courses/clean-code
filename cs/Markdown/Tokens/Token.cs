using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Markdown.Enums;

namespace Markdown.Tokens
{
    public class Token
    {
        public int Start { get; set; }
        public int End { get; set; }
        public TokenType Type { get; set; }
        public Token(int start, int end, TokenType type)
        {
            Start = start;
            End = end;
            Type = type;
        }
    }
}
