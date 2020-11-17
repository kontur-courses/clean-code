namespace Markdown
{
    public class Token
    {
        public int Position { get; }
        public string Value { get; set; }
        private int Length => Value.Length;
        public int EndPosition => Position + Length - 1;
        public TokenType Type { get; }

        public Token(int position, string value, TokenType type)
        {
            Value = value;
            Position = position;
            Type = type;
        }

        public string GetValueWithoutTags()
        {
            return Type switch
            {
                TokenType.Heading => Value[1..],
                TokenType.Emphasized => Value[1..^1],
                TokenType.Strong => Value[2..^2],
                _ => Value
            };
        }

        public bool IsIntersecting(Token token)
        {
            return !(Position > token.EndPosition
                   || EndPosition < token.Position);
        }

        public bool IsCollided(Token token)
        {
            return (Position < token.Position && EndPosition < token.EndPosition && EndPosition > token.Position)
                || (Position < token.EndPosition && EndPosition > token.EndPosition && Position > token.Position);
        }

        public bool IsInsideToken(Token token)
        {
            return Position >= token.Position
                   && EndPosition <= token.EndPosition;
        }
    }
}