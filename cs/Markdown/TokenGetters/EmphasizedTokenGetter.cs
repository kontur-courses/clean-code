using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Markdown
{
    public class EmphasizedTokenGetter : ITokenGetter
    {
        public TextToken TryGetToken(StringBuilder currentText, IReadOnlyCollection<ITokenGetter> tokenGetters,
            int index, string text)
        {
            if (!CanCreateToken(currentText, text, index))
                return null;

            currentText.Remove(0, 1);
            currentText.Remove(currentText.Length - 1, 1);
            var tokenToAdd = new TextToken(index - currentText.Length, currentText.Length,
                TokenType.Emphasized, currentText.ToString());
            var subTokens = new TextParser(tokenGetters).GetTextTokens(tokenToAdd.Text);
            tokenToAdd.SubTokens = subTokens;
            return tokenToAdd;
        }

        private static bool CanCreateToken(StringBuilder currentText, string text, int index)
        {
            return !IsTextContainsOnlyUnderlinings(currentText)
                   && IsTextStartAndEndWithOneUnderlining(currentText, text, index)
                   && !IsTextContainsNumbers(currentText)
                   && !IsSpaceAfterStartOrSpaceBeforeEnd(currentText)
                   && !IsStrongInsideEm(currentText)
                   && !IsUnderliningStartFromPartOfWordAndContainsSpace(currentText, index, text);
        }

        private static bool IsStrongInsideEm(StringBuilder currentText)
        {
            for (var i = 0; i < currentText.Length; i++)
            {
                if (currentText[i] == '_' && i + 1 < currentText.Length && currentText[i + 1] == '_')
                    return true;
            }

            return false;
        }

        private static bool IsTextContainsOnlyUnderlinings(StringBuilder currentText)
        {
            return currentText.ToString().Count(x => x == '_') == currentText.Length;
        }

        private static bool IsTextStartAndEndWithOneUnderlining(StringBuilder currentText, string text, int index)
        {
            return currentText[0] == '_' && currentText[1] != '_' && currentText[currentText.Length - 1] == '_'
                   && (index + 1 >= text.Length || text[index + 1] != '_');
        }

        private static bool IsTextContainsNumbers(StringBuilder currentText)
        {
            return currentText.ToString().Any(char.IsDigit);
        }

        private static bool IsSpaceAfterStartOrSpaceBeforeEnd(StringBuilder currentText)
        {
            return currentText[1] == ' ' || currentText[currentText.Length - 2] == ' ';
        }

        private static bool IsUnderliningStartFromPartOfWordAndContainsSpace(StringBuilder currentText, int index,
            string text)
        {
            return index - currentText.Length >= 0 && text[index - currentText.Length] != ' ' &&
                   currentText.ToString().Contains(' ');
        }
    }
}