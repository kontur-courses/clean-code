using System.Linq;

namespace Markdown
{
    public class EmphasizedTokenGetter : ITokenGetter
    {
        public TextToken GetToken(string text, int index, int startPosition)
        {
            var currentText = text.Substring(startPosition + 1, index - startPosition - 1);
            var tokenToAdd = new TextToken(startPosition, currentText.Length + 2,
                TokenType.Emphasized, currentText);
            return tokenToAdd;
        }

        public bool CanCreateToken(string text, int index, int startPosition)
        {
            var currentText = text.Substring(startPosition, index - startPosition + 1);
            return !IsTextContainsOnlyUnderlinings(currentText)
                   && IsTextStartAndEndWithOneUnderlining(currentText, text, index)
                   && !IsTextContainsNumbers(currentText)
                   && !IsSpaceAfterStartOrSpaceBeforeEnd(currentText)
                   && !IsStrongInsideEm(currentText)
                   && !IsUnderliningStartFromPartOfWordAndContainsSpace(currentText, index, text);
        }

        private static bool IsStrongInsideEm(string currentText)
        {
            for (var i = 0; i < currentText.Length; i++)
                if (currentText[i] == '_' && i + 1 < currentText.Length && currentText[i + 1] == '_')
                    return true;

            return false;
        }

        private static bool IsTextContainsOnlyUnderlinings(string currentText)
        {
            return currentText.All(x => x == '_');
        }

        private static bool IsTextStartAndEndWithOneUnderlining(string currentText, string text, int index)
        {
            return currentText[0] == '_' && currentText[1] != '_' && currentText[currentText.Length - 1] == '_'
                   && (index + 1 >= text.Length || text[index + 1] != '_');
        }

        private static bool IsTextContainsNumbers(string currentText)
        {
            return currentText.Any(char.IsDigit);
        }

        private static bool IsSpaceAfterStartOrSpaceBeforeEnd(string currentText)
        {
            return currentText[1] == ' ' || currentText[currentText.Length - 2] == ' ';
        }

        private static bool IsUnderliningStartFromPartOfWordAndContainsSpace(string currentText, int index,
            string text)
        {
            return index - currentText.Length >= 0 && text[index - currentText.Length] != ' ' &&
                   currentText.Contains(' ');
        }
    }
}