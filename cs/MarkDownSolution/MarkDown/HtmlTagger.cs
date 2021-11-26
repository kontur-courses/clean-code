using System.Collections.Generic;
using System.Text;

namespace MarkDown
{
    public static class HtmlTagger
    {
        private static readonly List<char> protectedChars = new() { '_', '\\'};
        private static int counter = 0;
        public static string GetString(Token inputToken, string text)
        {
            counter = 0;
            var SB = new StringBuilder(text);
            OpenToken(SB, inputToken);
            var nested = inputToken.GetNestedTokens();
            foreach (var token in nested)
            {
                ModifyStringBuilder(token, SB);
            }
            CloseToken(SB, inputToken);

            return GetStringWithoutEscapes(SB);
        }
        
        private static void ModifyStringBuilder(Token inputToken, StringBuilder SB)
        {
            OpenToken(SB, inputToken);
            var nested = inputToken.GetNestedTokens();
            foreach (var token in nested)
            {
                ModifyStringBuilder(token, SB);
            }
            CloseToken(SB, inputToken);
        }

        private static void CloseToken(StringBuilder SB, Token token)
        {
            var newStart = token.Start + counter + token.Length - token.RawLengthClose;
            SB.Remove(newStart, token.RawLengthClose);
            SB.Insert(newStart, token.ClosedHtmlTag);
            counter = counter - token.RawLengthClose + token.ClosedHtmlTag.Length;
        }

        private static void OpenToken(StringBuilder SB, Token token)
        {
            SB.Remove(token.Start + counter, token.RawLengthOpen);
            SB.Insert(token.Start + counter, token.OpenedHtmlTag);
            counter = counter - token.RawLengthOpen + token.OpenedHtmlTag.Length;
        }

        private static string GetStringWithoutEscapes(StringBuilder SB)
        {
            var str = SB.ToString();
            var SBResult = new StringBuilder();
            for (int i = 0; i < str.Length - 1; i++)
            {
                if (str[i] == '\\')
                {
                    if (protectedChars.Contains(str[i+1])) 
                    {
                        continue;
                    }
                }
                SBResult.Append(str[i]);
            }
            SBResult.Append(str[^1]);
            return SBResult.ToString();
        }
    }
}
