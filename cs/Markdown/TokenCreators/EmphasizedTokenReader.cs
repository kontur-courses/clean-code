using System.Linq;

namespace Markdown
{
    public class EmphasizedTokenReader : ITokenReader
    {
        public TextToken GetToken(string text, int index, int startPosition)
        {
            if (!CanCreateToken(text, index, startPosition))
                return null;
            
            var currentText = text.Substring(startPosition + 1, index - startPosition - 1);
            
            return new TextToken(startPosition, currentText.Length + 2,
                TokenType.Emphasized, currentText);
        }

        private static bool CanCreateToken(string text, int index, int startPosition)
        {
            var textToken = text.Substring(startPosition, index - startPosition + 1);
            return !IsTextContainsOnlyUnderlinings(textToken)
                   && IsTextStartAndEndWithOneUnderlining(textToken, text, index)
                   && !IsTextContainsNumbers(textToken)
                   && !IsSpaceAfterStartOrSpaceBeforeEnd(textToken)
                   && !IsStrongInsideEm(textToken)
                   && !IsUnderliningStartFromPartOfWordAndContainsSpace(textToken, index, text);
        }

        private static bool IsStrongInsideEm(string textToken)
        {
            for (var i = 0; i < textToken.Length; i++)
                if (textToken[i] == '_' && i + 1 < textToken.Length && textToken[i + 1] == '_')
                    return true;

            return false;
        }

        private static bool IsTextContainsOnlyUnderlinings(string textToken)
        {
            return textToken.All(x => x == '_');
        }

        private static bool IsTextStartAndEndWithOneUnderlining(string textToken, string text, int index)
        {
            return textToken[0] == '_' && textToken[1] != '_' && textToken[textToken.Length - 1] == '_'
                   && (index + 1 >= text.Length || text[index + 1] != '_');
        }

        private static bool IsTextContainsNumbers(string textToken)
        {
            return textToken.Any(char.IsDigit);
        }

        private static bool IsSpaceAfterStartOrSpaceBeforeEnd(string textToken)
        {
            return textToken[1] == ' ' || textToken[textToken.Length - 2] == ' ';
        }

        private static bool IsUnderliningStartFromPartOfWordAndContainsSpace(string textToken, int index,
            string text)
        {
            return index - textToken.Length >= 0 && text[index - textToken.Length] != ' ' &&
                   textToken.Contains(' ');
        }
    }
}