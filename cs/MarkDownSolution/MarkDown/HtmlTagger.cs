using System;
using System.Collections.Generic;
using System.Text;

namespace MarkDown
{
    public static class HtmlTagger
    {
        private static string italicOpen = "<em>";
        private static string italicClose = "</em>";
        private static string boldOpen = "<strong>";
        private static string boldClose = "</strong>";
        private static string headerOpen = "<h1>";
        private static string headerClose = "</h1>";
        private static List<char> protectedChars = new() { '_', '\\'};
        private static int counter = 0;
        public static string GetString(Token inputToken, string text)
        {
            counter = 0;
            var SB = new StringBuilder(text);
            HandleTokenOpen(SB, inputToken);
            var nested = inputToken.GetNestedTokens();
            foreach (var token in nested)
            {
                ModifyStringBuilder(token, SB);
            }
            HandleTokenClose(SB, inputToken);

            return GetStringWithoutEscapes(SB);
        }
        
        private static void ModifyStringBuilder(Token inputToken, StringBuilder SB)
        {
            HandleTokenOpen(SB, inputToken);
            var nested = inputToken.GetNestedTokens();
            foreach (var token in nested)
            {
                ModifyStringBuilder(token, SB);
            }
            HandleTokenClose(SB, inputToken);
        }

        private static void HandleTokenOpen(StringBuilder SB, Token token)
        {
            switch (token)
            {
                case ItalicToken:
                    OpenItalic(SB, token);
                    break;
                case BoldToken:
                    OpenBold(SB, token);
                    break;
                case HeaderToken:
                    OpenHeader(SB, token);
                    break;
            }
        }

        private static void HandleTokenClose(StringBuilder SB, Token token)
        {
            switch (token)
            {
                case ItalicToken:
                    CloseItalic(SB, token);
                    break;
                case BoldToken:
                    CloseBold(SB, token);
                    break;
                case HeaderToken:
                    CloseHeader(SB);
                    break;
            }
        }

        private static void CloseHeader(StringBuilder SB)
        {
            SB.Append(headerClose);
            counter += 5;
        }

        private static void OpenHeader(StringBuilder SB, Token token)
        {
            SB.Remove(token.start, 2);
            SB.Insert(token.start, headerOpen);
            counter += 2;
        }

        private static void CloseBold(StringBuilder SB, Token token)
        {
            SB.Remove(token.start + counter + token.length - 2, 2);
            SB.Insert(token.start + counter + token.length - 2, boldClose);
            counter += 7;
        }

        private static void OpenBold(StringBuilder SB, Token token)
        {
            SB.Remove(token.start + counter, 2);
            SB.Insert(token.start + counter, boldOpen);
            counter += 6;
        }

        private static void CloseItalic(StringBuilder SB, Token token)
        {
            SB.Remove(token.start + counter + token.length - 1, 1);
            SB.Insert(token.start + counter + token.length - 1, italicClose);
            counter += 4;
        }

        private static void OpenItalic(StringBuilder SB, Token token)
        {
            SB.Remove(token.start + counter, 1);
            SB.Insert(token.start + counter, italicOpen);
            counter += 3;
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
