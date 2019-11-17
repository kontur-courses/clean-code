using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public static class MarkdownToHtmlParser
    {
        public static List<Token> Parse(HashSet<Token> markdownTokens,
            Dictionary<Token, Token> tagsCollection)
        {
            var result = new List<Token>();
            var temp = new HashSet<Token>();
            var tokenStarts = GetTokenStarts(markdownTokens);
            foreach (var markdownToken in markdownTokens.Where(t => tokenStarts[t.Start] >= t.Start + t.Length).ToList())
            {
                var htmlToken = GetHtmlToken(markdownToken);
                result.Add(htmlToken);
                tagsCollection[markdownToken] = htmlToken;
                temp.Add(markdownToken);
            }

            foreach (var token in markdownTokens.Where(t=>!temp.Contains(t)))
            {
                var htmlString = GetHtmlToken(token).Line;
                var stringBuilder = new StringBuilder(htmlString);
                var delta = 0;
                foreach (var markdownToken in temp)
                {
                    stringBuilder.Replace(markdownToken.Line, tagsCollection[markdownToken].Line, markdownToken
                        .Start + delta - token.Start+6, markdownToken.Length);
                    delta += tagsCollection[markdownToken].Length - markdownToken.Length;
                }
                htmlString = stringBuilder.ToString();
                var htmlToken = new Token(htmlString, token.Start,htmlString.Length);
                result.Add(htmlToken);
                tagsCollection[token] = htmlToken;
            }
            return result;
        }

        private static Dictionary<int, int> GetTokenStarts(HashSet<Token> markdownTokens)
        {
            var tokensStarts = markdownTokens.OrderBy(t => t.Start).ToList();
            var result = new Dictionary<int, int>();
            for (int i = 0; i < tokensStarts.Count - 1; i++)
                result[tokensStarts[i].Start] = tokensStarts[i + 1].Start;
            var last = tokensStarts[tokensStarts.Count - 1];
            result[last.Start] = last.Start + last.Length;
            return result;
        }

        private static Token GetHtmlToken(Token markdownToken)
        {
            string htmlString;
            switch (markdownToken.Tag)
            {
                case "_":
                    htmlString = $"<em>{markdownToken.Line.Substring(1, markdownToken.Length - 2)}</em>";
                    return new Token(htmlString, markdownToken.Start, markdownToken.Length + 7);
                case "__":
                    htmlString = $"<strong>{markdownToken.Line.Substring(2, markdownToken.Length - 4)}</strong>";
                    return new Token(htmlString, markdownToken.Start, markdownToken.Length + 13);
            }
            throw new ArgumentException();
        }
    }
}