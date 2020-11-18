using System.Collections.Generic;

namespace Markdown
{
    public class TextToken
    {
        public TextToken(int length, TokenType type, string text, bool isTerminal)
        {
            Length = length;
            Type = type;
            Text = text;
            IsTerminal = isTerminal;
        }

        public TextToken(int length, TokenType type, string text, List<TextToken> subTokens)
        {
            Length = length;
            Type = type;
            Text = text;
            SubTokens = subTokens;
        }

        public int Length { get; set; }
        public TokenType Type { get; }
        public string Text { get; set; }
        public List<TextToken> SubTokens { get; set; }

        public bool IsTerminal { get; }
    }
}