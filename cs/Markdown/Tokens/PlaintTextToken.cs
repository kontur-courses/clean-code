using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class PlaintTextToken : TextToken
    {
        public PlaintTextToken(string text) : base(text.Length, TokenType.Text, null, true, null)
        {
            Text = RemoveShieldSymbols(text);
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