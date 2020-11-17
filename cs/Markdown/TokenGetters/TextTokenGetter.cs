using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class TextTokenGetter : ITokenGetter
    {
        public TextToken GetToken(string text, int index, int startPosition)
        {
            var currentText = text.Substring(startPosition, index - startPosition + 1);
            currentText = RemoveShieldSymbols(currentText);
            var tokenToAdd = new TextToken(startPosition, currentText.Length, TokenType.Text,
                currentText);
            return tokenToAdd;
        }

        public bool CanCreateToken(string text, int index, int startPosition)
        {
            var currentText = text.Substring(startPosition, index - startPosition + 1);
            return IsNextSymbolStartOfAnotherToken(currentText, text, index);
        }

        private static bool IsNextSymbolStartOfAnotherToken(string currentText, string text, int index)
        {
            if (index + 1 < text.Length && (currentText[0] == '_' || text[index + 1] != '_')) return false;
            if (index + 2 == text.Length && currentText[0] != '_' && text[index + 1] == '_') return false;
            if (!IsSpecialSymbolShielded(text, currentText, index))
                return false;
            if(!IsSubTokenContainsDigits(text, index))
                return false;
            return true;
        }

        private static bool IsSubTokenContainsDigits(string text, int index)
        {
            if (index + 2 < text.Length && char.IsDigit(text[index + 2])) return false;
            if (index + 3 < text.Length && char.IsDigit(text[index + 3])) return false;
            
            return true;
        }

        private static bool IsSpecialSymbolShielded(string text, string currentText, int index)
        {
            return index + 1 >= text.Length || currentText[currentText.Length - 1] != '\\' || text[index + 1] != '_';
        }
        
        private static string RemoveShieldSymbols(string currentText)
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

            return textWithoutShieldSymbols.ToString();
        }
        
        private static bool IsSpaceAfterStartOrSpaceBeforeEnd(string currentText)
        {
            return currentText[1] == ' ' || currentText[currentText.Length - 2] == ' ';
        }
    }
}