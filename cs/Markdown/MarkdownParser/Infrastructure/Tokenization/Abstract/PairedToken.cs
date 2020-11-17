namespace MarkdownParser.Infrastructure.Tokenization.Abstract
{
    public abstract class PairedToken : Token
    {
        public abstract TokenType Type { get; }

        protected PairedToken(int startPosition, string rawValue) : base(startPosition, rawValue)
        {
        }

        public bool CanBeOpening() => Type.HasFlag(TokenType.Opening);
        public bool CanBeClosing() => Type.HasFlag(TokenType.Closing);
    }
}