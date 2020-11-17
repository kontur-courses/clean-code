using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class TextTokenGetter : ITokenGetter
    {
        public TextToken TryGetToken(StringBuilder currentText, IReadOnlyCollection<ITokenGetter> tokenGetters,
            int index, string text)
        {
            currentText = RemoveShieldSymbols(currentText);
            var tokenToAdd = new TextToken(index - currentText.Length + 1, currentText.Length, TokenType.Text,
                currentText.ToString());
            return tokenToAdd;
        }

        public bool CanCreateToken(StringBuilder currentText, string text, int index)
        {
            return IsNextSymbolStartOfAnotherToken(currentText, text, index);
        }

        private static bool IsNextSymbolStartOfAnotherToken(StringBuilder currentText, string text, int index)
        {
            if (index + 1 < text.Length && (currentText[0] == '_' || text[index + 1] != '_')) return false;
            if (index + 2 == text.Length && currentText[0] != '_' && text[index + 1] == '_') return false;
            if (index + 1 < text.Length && currentText[currentText.Length - 1] == '\\' &&
                text[index + 1] == '_') return false;
            return true;
        }
        
        private static StringBuilder RemoveShieldSymbols(StringBuilder currentText)
        {
            var textWithoutShieldSymbols = new StringBuilder();
            for (var i = 0; i < currentText.Length; i++)
            {
                if (currentText[i] == '\\' && i + 1 < currentText.Length)
                {
                    if (currentText[i + 1] == '\\' || currentText[i + 1] == '_')
                        i++;
                }

                textWithoutShieldSymbols.Append(currentText[i]);
            }

            return textWithoutShieldSymbols;
        }
    }
}