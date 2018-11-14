namespace MarkDown
{
    public static class StringExtensions
    {
        public static char SeekFromPosition(this string text, int currentPosition, int delta) => 
            currentPosition + delta >= text.Length || currentPosition + delta < 0
            ? '\0'
            : text[currentPosition + delta];

        public static string RemoveScreening(this string text) => text.Replace("\\", "");

        public static bool IsOpeningTag(this string text, int startPosition, string specialSymbol)
        {
            var previousChar = text.SeekFromPosition(startPosition, -1);
            var nextChar = text.SeekFromPosition(startPosition, specialSymbol.Length);
            return (previousChar == '\0' || !char.IsDigit(previousChar))
                   && previousChar != '\\'
                   && nextChar != '\0'
                   && char.IsLetter(nextChar)
                   && text.Substring(startPosition).StartsWith(specialSymbol);
        }

        public static bool IsClosingTag(this string text, int startPosition, string specialSymbol)
        {

            var previousChar = text.SeekFromPosition(startPosition, -1);
            var nextChar = text.SeekFromPosition(startPosition, specialSymbol.Length);
            return (nextChar == '\0' || !char.IsDigit(nextChar)) 
                   && char.IsLetter(previousChar) 
                   && previousChar != '\\' 
                   && nextChar.ToString() != specialSymbol 
                   && text.Substring(startPosition).StartsWith(specialSymbol);
        }
    }
}
