using System.Linq;

namespace Markdown
{
    public class StrongTokenReader : ITokenReader
    {
        public TextToken TyrGetToken(string text, int index, int startPosition)
        {
            if (!CanCreateToken(text, index, startPosition))
                return null;

            var tokenText = text[(startPosition + 2)..(index - 1)];

            return new TextToken(tokenText.Length + 4,
                TokenType.Strong, tokenText, false);
        }

        private static bool CanCreateToken(string text, int index, int startPosition)
        {
            var tokenText = text[startPosition..(index + 1)];
            return tokenText.Any(x => x != '_')
                   && IsTextStartAndEndWithTwoUnderlinings(tokenText)
                   && !tokenText.Any(char.IsDigit)
                   && !FindCrossingUnderlinings(tokenText);
        }

        private static bool IsTextStartAndEndWithTwoUnderlinings(string tokenText)
        {
            if (tokenText[0] != '_' || tokenText[1] != '_' || tokenText[tokenText.Length - 2] != '_' ||
                tokenText[tokenText.Length - 1] != '_') return false;
            return true;
        }

        private static bool FindCrossingUnderlinings(string currentText)
        {
            var foundDoubleUnderlining = false;
            var foundUnderlining = false;
            for (var i = 0; i < currentText.Length; i++)
            {
                if (currentText[i] == '_' && i + 1 < currentText.Length && currentText[i + 1] == '_')
                {
                    if (foundUnderlining)
                        return true;
                    foundDoubleUnderlining = !foundDoubleUnderlining;
                    i++;
                    continue;
                }

                if (currentText[i] == '_' && i - 1 > -1 && currentText[i - 1] != '\\')
                    foundUnderlining = !foundUnderlining;
            }

            return false;
        }
    }
}