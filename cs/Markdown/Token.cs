using System.Collections.Generic;

namespace Markdown
{
    public class Token
    {
        public int Position { get; }
        public string Value { get; set; }
        public int EndPosition { get; }
        public TokenType Type { get; }
        public List<Token> ChildTokens { get; }

        public Token(int position, string value, int endPosition, TokenType type)
        {
            Value = value;
            EndPosition = endPosition;
            Position = position;
            Type = type;
            ChildTokens = new List<Token>();
        }
    }
}