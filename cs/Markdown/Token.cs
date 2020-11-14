using System;

namespace Markdown
{
    public class Token
    {
        public Token(TokenType type, string value, int startIndex /*, Token previous*/)
        {
            //Previous = previous;
            Type = type;
            Value = value;
            StartIndex = startIndex;
        }

        public TokenType Type { get; }
        public string Value { get; }
        public int StartIndex { get; }
        public int Length => Value.Length;
        public Token Next { get; private set; }

        internal void SetNext(Token next)
        {
            if (Next != null)
                throw new Exception("Next token is already set.");
            Next = next;
        }

        public static implicit operator string(Token token)
        {
            return token.Value;
        }

        public override string ToString()
        {
            return $"{Type} {StartIndex} {Value}";
        }
    }
}
