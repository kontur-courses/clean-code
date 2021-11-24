namespace Markdown.Tokens
{
    public class ContentToken : IToken
    {
        public string Value { get; }
        public TokenType Type => TokenType.Content;
        public int Position { get; }
        public bool IsOpening => false;
        public bool ShouldBeSkipped => true;
        public string OpeningTag => Value;
        public string ClosingTag => Value;

        public ContentToken(string value, int position)
        {
            Value = value;
            Position = position;
        }
    }
}
