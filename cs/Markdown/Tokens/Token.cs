using System;

namespace Markdown.Tokens
{
    public class Token
    {
        public Token(TokenType tokenType, string value)
        {
            TokenType = tokenType;
            Value = value;
        }

        public TokenType TokenType { get; }
        public string Value { get; }

        private bool Equals(Token other)
        {
            return TokenType == other.TokenType && Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Token) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine((int) TokenType, Value);
        }
    }
}