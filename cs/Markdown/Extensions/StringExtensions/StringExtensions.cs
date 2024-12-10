namespace Markdown.Extensions.HandlerExtensions
{
    internal static class StringExtensions
    {
        internal static int FindEndOfLine(this string text, int startIndex)
        {
            var result = text.IndexOf('\n', startIndex);
            if (result == -1)
            {
                return text.Length;
            }
            return result;
        }

        internal static bool IsSurroundedByDigits(this string text, int index, int markerLength)
        {
            var isLeftDigit = index - 1 >= 0 && char.IsDigit(text[index - 1]);
            var isRightDigit = index + markerLength < text.Length
                && char.IsDigit(text[index + markerLength]);
            return isLeftDigit || isRightDigit;
        }

        internal static int CountConsecutiveCharacters(this string text, int index, char character)
        {
            var result = 0;
            while (index + result < text.Length && text[index + result] == character)
            {
                result += 1;
            }
            return result;
        }

        internal static bool IsInsideOfWord(this string text, int index, int markerLength)
            => index - markerLength >= 0
            && char.IsLetter(text[index - markerLength])
            && index + markerLength < text.Length
            && char.IsLetter(text[index + markerLength]);

        internal static int FindClosingMarker(this string text, int startIndex, string marker)
        {
            for (var i = startIndex; i < text.Length; i++)
            {
                if (char.IsWhiteSpace(text[i]))
                {
                    var nextIndex = text.FindClosingMarker(i + 1, marker);
                    if (text.IsInsideOfWord(nextIndex, marker.Length))
                    {
                        return -1;
                    };
                }

                if (i + marker.Length > text.Length)
                {
                    return -1;
                }

                var consecutiveCharactersCount = text.CountConsecutiveCharacters(i, marker[0]);
                var sub = text.Substring(i, marker.Length);
                if (sub == marker && consecutiveCharactersCount == marker.Length)
                {
                    var preMarkerIndex = i - 1;
                    if (preMarkerIndex >= startIndex && !char.IsWhiteSpace(text[preMarkerIndex]))
                    {
                        return i;
                    }
                }
                i += consecutiveCharactersCount;
            }

            return -1;
        }
    }
}
