using Markdown.Tokens;

namespace Markdown.TokenReaders
{
    public class PlainTextTokenReader : ITokenReader
    {
        public TextToken TyrGetToken(string text, int start, int end)
        {
            return !CanCreateToken(text, end, start) ? null : new PlainTextToken(text[start..(end + 1)]);
        }

        private static bool CanCreateToken(string text, int index, int startPosition)
        {
            var tokenText = text[startPosition..(index + 1)];
            return IsSpecialSymbolShielded(text, tokenText, index)
                   && IsSubTokenContainsDigits(text, index)
                   && IsNextSymbolStartOfAnotherToken(tokenText, text, index);
        }

        private static bool IsNextSymbolStartOfAnotherToken(string tokenText, string text, int index)
        {
            return (index + 1 >= text.Length || tokenText[0] != '_' && text[index + 1] == '_') &&
                   (index + 2 != text.Length || tokenText[0] == '_' || text[index + 1] != '_')
                   && (index + 1 >= text.Length || tokenText[0] != '[' && text[index + 1] != ']');
        }

        private static bool IsSubTokenContainsDigits(string text, int index)
        {
            return (index + 2 >= text.Length || !char.IsDigit(text[index + 2])) &&
                   (index + 3 >= text.Length || !char.IsDigit(text[index + 3]));
        }

        private static bool IsSpecialSymbolShielded(string text, string tokenText, int index)
        {
            return index + 1 >= text.Length || tokenText[tokenText.Length - 1] != '\\' || text[index + 1] != '_';
        }
    }
}