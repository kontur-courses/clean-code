using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class StrongTokenGetter : ITokenGetter
    {
        public TextToken TryGetToken(StringBuilder currentText, IReadOnlyCollection<ITokenGetter> tokenGetters,
            int index, string text)
        {
            currentText.Remove(0, 2);
            currentText.Remove(currentText.Length - 2, 2);
            var tokenToAdd = new TextToken(index - currentText.Length - 1, currentText.Length,
                TokenType.Strong, currentText.ToString());
            tokenToAdd.SubTokens = new TextParser(tokenGetters).GetTextTokens(tokenToAdd.Text);
            return tokenToAdd;
        }

        public bool CanCreateToken(StringBuilder currentText, string text, int index)
        {
            if (currentText.ToString().Count(x => x == '_') == currentText.Length)
                return false;
            if (currentText[0] != '_' || currentText[1] != '_' || currentText[currentText.Length - 2] != '_' ||
                currentText[currentText.Length - 1] != '_') return false;
            if (currentText.ToString().Any(char.IsDigit))
                return false;
            if (FindCrossingUnderlinings(currentText))
                return false;
            return true;
        }

        private static bool FindCrossingUnderlinings(StringBuilder currentText)
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