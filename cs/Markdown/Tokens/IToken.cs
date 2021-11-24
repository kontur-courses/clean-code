namespace Markdown.Tokens
{
    public interface IToken
    {
        public string Value { get; }
        public TokenType Type { get; }
        public int Length => Value.Length;
        public int Position { get; }
        public bool IsOpening { get; }
        public bool ShouldBeSkipped { get; }
        public string OpeningTag { get; }
        public string ClosingTag { get; }
    }
}
