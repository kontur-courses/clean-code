using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class TextToken 
    {
        public int StartPosition { get; set; }
        public int Length { get; set; }
        public TokenType Type { get; set; }
        public string Text { get; set; }
        public List<TextToken> SubTokens { get; set; }

        public TextToken(int startPosition, int length, TokenType type, string text)
        {
            StartPosition = startPosition;
            Length = length;
            Type = type;
            Text = text;
        }
        
        public TextToken(int startPosition, int length, TokenType type, string text, List<TextToken> subTokens)
        {
            StartPosition = startPosition;
            Length = length;
            Type = type;
            Text = text;
            SubTokens = subTokens;
        }

        public TextToken GetLastSubToken()
        {
            return SubTokens == null ? null : SubTokens.LastOrDefault();
        }
    }
}
