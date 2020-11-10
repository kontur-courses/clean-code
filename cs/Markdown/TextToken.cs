using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public struct TextToken 
    {
        public int StartPosition { get; }
        public int Length { get; }
        public TokenType Type { get; }

        public TextToken(int startPosition, int length, TokenType type)
        {
            StartPosition = startPosition;
            Length = length;
            Type = type;
        }
    }
}
