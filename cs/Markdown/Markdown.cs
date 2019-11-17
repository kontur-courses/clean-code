using Markdown.Tokens;
using System.Collections.Generic;

namespace Markdown
{
    public class Md
    {
        public string Render(string mdText)
        {
            if (string.IsNullOrWhiteSpace(mdText))
                return mdText;

            var rootToken = new Root(ref mdText);
            ParseTokenText(rootToken, ref mdText);

            return TextConverter.HTMLConverter().Convert(rootToken, ref mdText);
        }

        private void ParseTokenText(Token parentToken, ref string document)
        {
            var i = parentToken.BeginIndex;
            while (i < parentToken.EndIndex)
            {
                if (TokenRules.IsTokenTagInPosition(i, ref document, out List<Token> matchedTokens))
                {
                    var isTokenAdded = false;
                    foreach (var matchedToken in matchedTokens)
                    {
                        if (matchedToken.CanBeBeginned(i, ref document, parentToken))
                        {
                            parentToken.Children.Add(matchedToken);
                            ParseTokenText(matchedToken, ref document);
                            i = matchedToken.EndIndex;
                            isTokenAdded = true;
                            break;
                        }
                    }
                    if (!isTokenAdded)
                    {
                        var rawText = new RawText
                        {
                            BeginIndex = i,
                            EndIndex = i + matchedTokens[0].MarkdownTag.Length
                        };
                        parentToken.Children.Add(rawText);
                        i = rawText.EndIndex;
                    }
                }
                else
                    i++;
            }

            if (parentToken.Children.Count > 0)
            {
                i = parentToken.BeginIndex;
                while (i < parentToken.EndIndex)
                {
                    //TODO raw between other tokens
                }
            }
        }        
    }
}
