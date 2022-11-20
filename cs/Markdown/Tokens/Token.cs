using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.Tags;

namespace Markdown.Tokens
{
    public class Token
    {
        public TokenType Type { get; }

        public int Start { get; }

        public int Length { get; }

        public int End => Start + Length - 1;

        public Token(TokenType type, int start, int length)
        {
            Type = type;
            Start = start;
            Length = length;
        }
    }
}
