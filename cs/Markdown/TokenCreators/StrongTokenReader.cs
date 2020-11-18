using System.Linq;

namespace Markdown
{
    public class StrongTokenReader : ITokenReader
    {
        public TextToken GetToken(string text, int index, int startPosition)
        {
            if (!CanCreateToken(text, index, startPosition))
                return null;
            
            var tokenText = text.Substring(startPosition + 2, index - startPosition - 3);
            
            return new TextToken(startPosition, tokenText.Length + 4,
                TokenType.Strong, tokenText);
        }

        private static bool CanCreateToken(string text, int index, int startPosition)
        {
            var tokenText = text.Substring(startPosition, index - startPosition + 1);
            if (tokenText.All(x => x == '_'))
                return false;
            if (tokenText[0] != '_' || tokenText[1] != '_' || tokenText[tokenText.Length - 2] != '_' ||
                tokenText[tokenText.Length - 1] != '_') return false;
            if (tokenText.Any(char.IsDigit))
                return false;
            if (FindCrossingUnderlinings(tokenText))
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