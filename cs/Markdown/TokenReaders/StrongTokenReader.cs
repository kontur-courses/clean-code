using System.Linq;
using Markdown.Tokens;

namespace Markdown.TokenReaders
{
    public class StrongTokenReader : ITokenReader
    {
        public TextToken TyrGetToken(string text, int start, int end)
        {
            return !CanCreateToken(text, end, start) ? null : new StrongTextToken(text[start..(end + 1)]);
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
            return tokenText[0] == '_' && tokenText[1] == '_'
                                       && tokenText[tokenText.Length - 2] == '_'
                                       && tokenText[tokenText.Length - 1] == '_';
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