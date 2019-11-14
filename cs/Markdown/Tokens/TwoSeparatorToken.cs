namespace Markdown.Tokens
{
    public class TwoSeparatorToken : Token
    {
        public string Separator { get; }

        public string ValueWithSeparators => $"{Separator}{Value}{Separator}";

        public int FirstSeparatorPosition => Position - Separator.Length;

        public TwoSeparatorToken(int position, string value, string separator) : base(position, value)
        {
            Separator = separator;
        }

        public static TwoSeparatorToken FromSeparatorPositions(string text, int firstSeparatorPos, int secondSeparatorPos,
            string separatorValue)
        {
            var startPosition = firstSeparatorPos + separatorValue.Length;
            return new TwoSeparatorToken(startPosition, text.Substring(startPosition, secondSeparatorPos - startPosition),
                separatorValue);
        }
    }
}