namespace Markdown.Tokens
{
    public class HeadingToken : IToken
    {
        public string Value => "# ";
        public TokenType Type => TokenType.Heading;
        public int Position { get; }
        public bool IsOpening { get; }
        public bool ShouldBeSkipped => false;
        public string OpeningTag => "<h1>";
        public string ClosingTag => "</h1>";

        public HeadingToken(int position, bool isOpening)
        {
            Position = position;
            IsOpening = isOpening;
        }
    }
}
