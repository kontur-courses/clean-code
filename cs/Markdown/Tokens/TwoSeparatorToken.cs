namespace Markdown.Tokens
{
    public class TwoSeparatorToken : Token
    {
        public string Separator { get; }

        public string TokenValueWithSeparators => $"{Separator}{TokenValue}{Separator}";

        public TwoSeparatorToken(int position, int length, string value, string separator) : base(position, length, value)
        {
            Separator = separator;
        }

        public static TwoSeparatorToken FromSeparatorPositions(string text, int firstSeparatorPos, int secondSeparatorPos,
            string separatorValue)
        {
            var startPosition = firstSeparatorPos + separatorValue.Length;
            return new TwoSeparatorToken(startPosition, secondSeparatorPos - startPosition, text, separatorValue);
        }
    }
}