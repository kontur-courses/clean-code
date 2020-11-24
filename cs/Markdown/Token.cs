using System;

namespace Markdown
{
    public class Token
    {
        public Token(TokenType type, string value, int startIndex, Token previous, int line)
        {
            Previous = previous;
            Line = line;
            Type = type;
            Value = value;
            StartIndex = startIndex;
        }

        public TokenType Type { get; }
        public string Value { get; }
        public int StartIndex { get; }
        public int Line { get; }
        public int Length => Value.Length;
        public Token Next { get; private set; }
        public Token NextSomeType { get; private set; }
        public Token NextLine { get; private set; }
        public Token Previous { get; }
        public bool IgnoreAsTag { get; private set; }

        public static implicit operator string(Token token)
        {
            return token.Value;
        }

        public override string ToString()
        {
            return $"{Type} {StartIndex} {Value}";
        }

        public Token TagIgnore()
        {
            IgnoreAsTag = true;
            return this;
        }

        internal void SetNext(Token next)
        {
            CheckAlreadySet(Next);
            Next = next;
        }

        internal void SetSomeNext(Token token)
        {
            CheckAlreadySet(NextSomeType);
            NextSomeType = token;
        }

        internal void SetNextLine(Token token)
        {
            CheckAlreadySet(NextLine);
            NextLine = token;
        }

        private static void CheckAlreadySet(Token value)
        {
            if (value != null)
                throw new Exception("Current token is already set.");
        }
    }
}
