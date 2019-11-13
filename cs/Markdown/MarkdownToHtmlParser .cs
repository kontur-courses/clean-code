using System;
using System.Collections.Generic;

namespace Markdown
{
    public static class MarkdownToHtmlParser
    {
        public static List<Token> Parse(List<Token> MarkdownTokens,
            Dictionary<Token, Token> tagsColletion)
        {
            var result= new List<Token>();
            foreach (var markdownToken in MarkdownTokens)
            {
                if (markdownToken.Line[0]=='_'&&markdownToken.Line[1] == '_')
                {
                    var htmlString = string.Format("<strong>{0}</strong>",
                        markdownToken.Line.Substring(2, markdownToken.Length - 4));
                    var htmlToken = new Token(htmlString, markdownToken.Start, markdownToken.Length + 13);
                    result.Add(htmlToken);
                    tagsColletion[markdownToken] = htmlToken;
                    continue;
                }

                if (markdownToken.Line[0] == '_' && markdownToken.Line[1] != ' ')
                {
                    var htmlString = string.Format("<em>{0}</em>",
                        markdownToken.Line.Substring(1, markdownToken.Length - 2));
                    var htmlToken = new Token(htmlString, markdownToken.Start, markdownToken.Length + 7);
                    result.Add(htmlToken);
                    tagsColletion[markdownToken] = htmlToken;
                    continue;
                }
            }

            return result;
        }
    }
}