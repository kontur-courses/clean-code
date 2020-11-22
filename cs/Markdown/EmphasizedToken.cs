using System.Collections.Generic;

namespace Markdown
{
    public class EmphasizedToken : IToken
    {
        public EmphasizedToken(int position, string value, int endPosition)
        {
            Position = position;
            Value = value;
            EndPosition = endPosition;
            Type = TokenType.Emphasized;
            ChildTokens = new List<IToken>();
            CanHaveChildTokens = true;
        }

        public int Position { get; }
        public string Value { get; }
        public int EndPosition { get; }
        public TokenType Type { get; }
        public List<IToken> ChildTokens { get; }
        public bool CanHaveChildTokens { get; }
    }
}