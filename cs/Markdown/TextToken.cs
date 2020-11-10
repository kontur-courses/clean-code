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
        public string Text;

        public TextToken(int startPosition, int length, TokenType type, string text)
        {
            StartPosition = startPosition;
            Length = length;
            Type = type;
            Text = text;
        }
    }
}
