using System;
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
        public static string GetString(Token inputToken, string text)
        {
            //главный токен тоже проверять и делать рекурсию.
            var counter = 0;
            var SB = new StringBuilder(text);
            var nested = inputToken.GetNestedTokens();
            foreach (var token in nested)
            {
                if (token is ItalicToken)
                {
                    SB.Remove(token.start + counter, 1);
                    SB.Insert(token.start + counter, italicOpen);
                    counter += 3;
                    SB.Remove(token.start + counter + token.length - 1, 1);
                    SB.Insert(token.start + counter + token.length - 1, italicClose);
                    counter += 4;
                }
                else if (token is BoldToken)
                {
                    SB.Remove(token.start + counter, 2);
                    SB.Insert(token.start + counter, boldOpen);
                    counter += 6;
                    SB.Remove(token.start + counter + token.length - 2, 2);
                    SB.Insert(token.start + counter + token.length - 2, boldClose);
                    counter += 7;
                }
                else if (token is HeaderToken)
                {
                    SB.Remove(token.start, 2);
                    SB.Insert(token.start, headerOpen);
                    counter += 2;
                    SB.Append(headerClose);
                }
            }
            return SB.ToString();
        }
    }
}
