using System.Collections.Generic;

namespace Markdown
{
    public class PlaintTextToken : IToken
    {
        public PlaintTextToken(int position, string value, int endPosition)
        {
            Position = position;
            Value = value;
            EndPosition = endPosition;
            Type = TokenType.PlainText;
            ChildTokens = new List<IToken>();
            CanHaveChildTokens = false;
        }

        public int Position { get; }
        public string Value { get; }
        public int EndPosition { get; }
        public TokenType Type { get; }
        public List<IToken> ChildTokens { get; }
        public bool CanHaveChildTokens { get; }
    }
}