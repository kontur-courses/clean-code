using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class Md2HtmlTranslator
    {
        public string TranslateMdToHtml(string mdText, Dictionary<TokenType, List<TokenPosition>> positionsForTokensTypes)
        {
            var sortedPositions = GetTokensSortedByPosition(positionsForTokensTypes);
            var htmlText = GetHtmlText(mdText, sortedPositions);

            return htmlText;
        }

        private string GetHtmlText(string mdText, IEnumerable<SingleToken> tokensStream)
        {
            var htmlBuilder = new StringBuilder();
            var lastIndex = 0;

            foreach (var token in tokensStream)
            {
                var htmlTag = token.LocationType == LocationType.Opening ? $"<{token.TokenType.HtmlTag}>" :
                    token.LocationType == LocationType.Closing ? $"</{token.TokenType.HtmlTag}>" :
                    throw new InvalidOperationException("Invalid token location type");

                htmlBuilder.Append(mdText.Substring(lastIndex, token.TokenPosition - lastIndex));
                htmlBuilder.Append(htmlTag);

                lastIndex = token.TokenPosition + token.TokenType.Template.Length;
            }

            return htmlBuilder.ToString();
        }

        private IEnumerable<SingleToken> GetTokensSortedByPosition(Dictionary<TokenType, List<TokenPosition>> positionsForTokensTypes)
        {
            var sortedPositionsWithTokens = new List<SingleToken>();

            foreach (var tokenWithPositions in positionsForTokensTypes)
            foreach (var position in tokenWithPositions.Value)
            {
                sortedPositionsWithTokens.Add
                    (new SingleToken(tokenWithPositions.Key, position.Start, LocationType.Opening));

                sortedPositionsWithTokens.Add
                    (new SingleToken(tokenWithPositions.Key, position.End, LocationType.Closing));
            }

            return sortedPositionsWithTokens.OrderBy(token => token.TokenPosition);
        }
    }
}
