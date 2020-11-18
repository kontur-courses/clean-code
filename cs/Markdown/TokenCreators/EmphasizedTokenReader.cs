using System.Linq;

namespace Markdown
{
    public class EmphasizedTokenReader : ITokenReader
    {
        public TextToken TyrGetToken(string text, int end, int start)
        {
            if (!CanCreateToken(text, end, start))
                return null;

            var tokenText = text[start..(end + 1)];

            return new EmphasizedTextToken(tokenText);
        }

        private static bool CanCreateToken(string text, int end, int start)
        {
            var tokenText = text[start..(end + 1)];
            return !IsTextContainsOnlyUnderlinings(tokenText)
                   && IsTextStartAndEndWithOneUnderlining(tokenText, text, end)
                   && !IsTextContainsNumbers(tokenText)
                   && !IsSpaceAfterStartOrSpaceBeforeEnd(tokenText)
                   && !IsStrongInsideEm(tokenText)
                   && !(IsUnderliningStartFromPartOfWord(tokenText, end, text)
                        && IsTextContainsSpace(tokenText));
        }

        private static bool IsStrongInsideEm(string tokenText)
        {
            for (var i = 0; i < tokenText.Length; i++)
                if (tokenText[i] == '_' && i + 1 < tokenText.Length && tokenText[i + 1] == '_')
                    return true;

            return false;
        }

        private static bool IsTextContainsOnlyUnderlinings(string tokenText)
        {
            return tokenText.All(x => x == '_');
        }

        private static bool IsTextStartAndEndWithOneUnderlining(string tokenText, string text, int end)
        {
            return tokenText[0] == '_' && tokenText[1] != '_' && tokenText[tokenText.Length - 1] == '_'
                   && (end + 1 >= text.Length || text[end + 1] != '_');
        }

        private static bool IsTextContainsNumbers(string tokenText)
        {
            return tokenText.Any(char.IsDigit);
        }

        private static bool IsSpaceAfterStartOrSpaceBeforeEnd(string tokenText)
        {
            return tokenText[1] == ' ' || tokenText[tokenText.Length - 2] == ' ';
        }

        private static bool IsUnderliningStartFromPartOfWord(string tokenText, int end,
            string text)
        {
            return end - tokenText.Length >= 0 && text[end - tokenText.Length] != ' ';
        }

        private static bool IsTextContainsSpace(string tokenText)
        {
            return tokenText.Contains(' ');
        }
    }
}