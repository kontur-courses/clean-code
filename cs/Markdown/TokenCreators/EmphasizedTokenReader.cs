using System.Linq;

namespace Markdown
{
    public class EmphasizedTokenReader : ITokenReader
    {
        public TextToken TyrGetToken(string text, int index, int startPosition)
        {
            if (!CanCreateToken(text, index, startPosition))
                return null;

            var tokenText = text[(startPosition + 1)..index];

            return new TextToken(tokenText.Length + 2,
                TokenType.Emphasized, tokenText, false);
        }

        private static bool CanCreateToken(string text, int index, int startPosition)
        {
            var tokenText = text[startPosition..(index + 1)];
            return !IsTextContainsOnlyUnderlinings(tokenText)
                   && IsTextStartAndEndWithOneUnderlining(tokenText, text, index)
                   && !IsTextContainsNumbers(tokenText)
                   && !IsSpaceAfterStartOrSpaceBeforeEnd(tokenText)
                   && !IsStrongInsideEm(tokenText)
                   && !(IsUnderliningStartFromPartOfWord(tokenText, index, text)
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

        private static bool IsTextStartAndEndWithOneUnderlining(string tokenText, string text, int index)
        {
            return tokenText[0] == '_' && tokenText[1] != '_' && tokenText[tokenText.Length - 1] == '_'
                   && (index + 1 >= text.Length || text[index + 1] != '_');
        }

        private static bool IsTextContainsNumbers(string tokenText)
        {
            return tokenText.Any(char.IsDigit);
        }

        private static bool IsSpaceAfterStartOrSpaceBeforeEnd(string tokenText)
        {
            return tokenText[1] == ' ' || tokenText[tokenText.Length - 2] == ' ';
        }

        private static bool IsUnderliningStartFromPartOfWord(string tokenText, int index,
            string text)
        {
            return index - tokenText.Length >= 0 && text[index - tokenText.Length] != ' ';
        }

        private static bool IsTextContainsSpace(string tokenText)
        {
            return tokenText.Contains(' ');
        }
    }
}