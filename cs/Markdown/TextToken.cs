using System.Collections.Generic;

namespace Markdown
{
    public class TextToken
    {
        public int StartPosition { get; }
        public int Length { get; set; }
        public TokenType Type { get; }
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
    }
}