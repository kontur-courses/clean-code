namespace Markdown.Tokens
{
    public class BoldToken : IToken
    {
        public const char FirstSymbol = '_';
        public const char SecondSymbol = '_';

        public string Value => "__";
        public TokenType Type => TokenType.Bold;
        public int Position { get; }
        public bool IsOpening { get; set; }
        public bool ShouldBeSkipped { get; set; }
        public string OpeningTag => "<strong>";
        public string ClosingTag => "</strong>";

        public BoldToken(int position, bool isOpening, bool shouldBeSkipped)
        {
            Position = position;
            IsOpening = isOpening;
            ShouldBeSkipped = shouldBeSkipped;
        }

        public BoldToken(int position)
        {
            Position = position;
        }
    }
}
