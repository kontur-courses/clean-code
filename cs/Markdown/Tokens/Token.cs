using System.Collections.Generic;

namespace Markdown.Tokens
{
    public class Token
    {
        protected Token(in int position, string value, in int endPosition, TokenType type, bool canHaveChildTokens)
        {
            Position = position;
            Value = value;
            EndPosition = endPosition;
            Type = type;
            ChildTokens = new List<Token>();
            CanHaveChildTokens = canHaveChildTokens;
        }

        public int Position { get; }
        public string Value { get; }
        public int EndPosition { get; }
        public TokenType Type { get; }
        public List<Token> ChildTokens { get; }
        public bool CanHaveChildTokens { get; }
    }
}