namespace Markdown
{
    public class Token
    {
        public string Value { get; }
        public TokenType Type { get; }
        public int Length => Value.Length;
        public int Position { get; }
        public bool IsOpening { get; }

        public Token(string value, TokenType type, int position, bool isOpening)
        {
            Value = value;
            Type = type;
            Position = position;
            IsOpening = isOpening;
        }
    }
}
