namespace Markdown.Core.Tokens
{
    public class Token : IToken
    {
        public int Position { get; }
        public int Length { get; }
        public string Value { get; }
        public TokenType TokenType { get; set; }

        protected Token(int position, string value, TokenType tokenType)
        {
            Position = position;
            Value = value;
            Length = value.Length;
            TokenType = tokenType;
        }

        public override bool Equals(object obj)
        {
            var other = obj as IToken;
            if (other == null)
                return false;
            return Equals(other);
        }

        private bool Equals(IToken other)
        {
            return Position == other.Position &&
                   Length == other.Length &&
                   string.Equals(Value, other.Value) &&
                   TokenType == other.TokenType;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Position;
                hashCode = (hashCode * 397) ^ Length;
                hashCode = (hashCode * 397) ^ (Value != null ? Value.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int) TokenType;
                return hashCode;
            }
        }
    }
}