using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class StrongTokenGetter : ITokenGetter
    {
        public TextToken GetToken(string text, int index, int startPosition)
        {
            var currentText = text.Substring(startPosition + 2, index - startPosition - 3);
            var tokenToAdd = new TextToken(startPosition, currentText.Length + 4,
                TokenType.Strong, currentText);
            return tokenToAdd;
        }

        public bool CanCreateToken(string text, int index, int startPosition)
        {
            var currentText = text.Substring(startPosition, index - startPosition + 1);
            if (currentText.Count(x => x == '_') == currentText.Length)
                return false;
            if (currentText[0] != '_' || currentText[1] != '_' || currentText[currentText.Length - 2] != '_' ||
                currentText[currentText.Length - 1] != '_') return false;
            if (currentText.ToString().Any(char.IsDigit))
                return false;
            if (FindCrossingUnderlinings(currentText))
                return false;
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