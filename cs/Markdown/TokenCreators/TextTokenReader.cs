using System.Text;

namespace Markdown
{
    public class TextTokenReader : ITokenReader
    {
        public TextToken GetToken(string text, int index, int startPosition)
        {
            if (!CanCreateToken(text, index, startPosition))
                return null;

            var tokenText = text.Substring(startPosition, index - startPosition + 1);
            /*Range myrange = 1..4;
            var tokenText = text[index..index - startPosition + 1];*/
            var tokenTextWithoutShields = RemoveShieldSymbols(tokenText);

            return new TextToken(startPosition, tokenTextWithoutShields.Length, TokenType.Text,
                tokenTextWithoutShields);
        }

        private static bool CanCreateToken(string text, int index, int startPosition)
        {
            var tokenText = text.Substring(startPosition, index - startPosition + 1);
            return IsSpecialSymbolShielded(text, tokenText, index) && IsSubTokenContainsDigits(text, index) &&
                   IsNextSymbolStartOfAnotherToken(tokenText, text, index);
        }

        private static bool IsNextSymbolStartOfAnotherToken(string tokenText, string text, int index)
        {
            return (index + 1 >= text.Length || tokenText[0] != '_' && text[index + 1] == '_') &&
                   (index + 2 != text.Length || tokenText[0] == '_' || text[index + 1] != '_');
        }

        private static bool IsSubTokenContainsDigits(string text, int index)
        {
            return (index + 2 >= text.Length || !char.IsDigit(text[index + 2])) &&
                   (index + 3 >= text.Length || !char.IsDigit(text[index + 3]));
        }

        private static bool IsSpecialSymbolShielded(string text, string tokenText, int index)
        {
            return index + 1 >= text.Length || tokenText[tokenText.Length - 1] != '\\' || text[index + 1] != '_';
        }

        private static string RemoveShieldSymbols(string tokenText)
        {
            var textWithoutShieldSymbols = new StringBuilder();
            for (var i = 0; i < tokenText.Length; i++)
            {
                if (IsCurrentSymbolShieldAndNextSymbolIsSpecial(tokenText, i))
                    i++;

                textWithoutShieldSymbols.Append(tokenText[i]);
            }

            return textWithoutShieldSymbols.ToString();
        }

        private static bool IsCurrentSymbolShieldAndNextSymbolIsSpecial(string tokenText, int i)
        {
            return tokenText[i] == '\\' && i + 1 < tokenText.Length &&
                   (tokenText[i + 1] == '\\' || tokenText[i + 1] == '_');
        }
    }
}