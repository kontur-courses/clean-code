using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Programm
{
    public class Md
    {
        private static readonly HashSet<string> markingElements = new HashSet<string>()
        {
            "_"
        };

        public static string Render(string markDownParagraph)
        {
            var rawTokens = markDownParagraph.Select(c => new Token(c.ToString(),
                c.ToString(), false)).ToArray();
            var paragraphInTokens = MakeEscapeCharsAndGetTokens(rawTokens);

            var UnderScoresHandledTokens = UnderScoresHandler.HandleUnderScores(paragraphInTokens);
            var htmlParagraph = GetHtmlTextFromTokens(UnderScoresHandledTokens);
            return htmlParagraph;
        }

        private static Token[] MakeEscapeCharsAndGetTokens(Token[] rawTokens) //TODO Refactoring
        {
            var tokenList = new List<Token>();
            var len = rawTokens.Length;
            for (var i = 0; i < len; )
            {
                var curToken = rawTokens[i];
                if (curToken.OriginalValue != "\\")
                {
                    if (curToken.IsEscapeChar)
                        curToken.RenderedValue = curToken.OriginalValue;
                    tokenList.Add(curToken);
                    i++;
                    continue;
                }
                var slashCnt = 0;
                while (i < len && rawTokens[i].OriginalValue == "\\")
                {
                    i++;
                    slashCnt++;
                }
                for (var j = 0; j < slashCnt / 2; j++)
                    tokenList.Add(new Token("\\", "\\", true));
                if (slashCnt % 2 != 0)
                {
                    if (i >= len || !markingElements.Contains(rawTokens[i].OriginalValue))
                        tokenList.Add(new Token("\\", "\\", true));
                    else
                        rawTokens[i].IsEscapeChar = true;
                }
            }
            return tokenList.ToArray();
        }

        private static string GetHtmlTextFromTokens(Token[] tokens)
        {
            var s = new StringBuilder();
            foreach (var token in tokens)
                s.Append(token.RenderedValue);
            return s.ToString();
        }
    }
}