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
            var sortedPositions = GetSortedPositionsWithTags(positionsForTokensTypes);
            return GetHtmlText(mdText, sortedPositions);
        }

        private string GetHtmlText(string mdText, List<SingleToken> tokensStream)
        {
            var htmlBuilder = new StringBuilder();
            var lastIndex = 0;
            foreach (var token in tokensStream.OrderBy(token => token.TokenPosition))
            {
                htmlBuilder.Append(mdText.Substring(lastIndex, token.TokenPosition - lastIndex));
                var htmlTag = token.LocationType == LocationType.Opening ? $"<{token.TokenType.HtmlTag}>" :
                    token.LocationType == LocationType.Closing ? $"</{token.TokenType.HtmlTag}>" :
                    throw new InvalidOperationException("Invalid token location type");
                htmlBuilder.Append(htmlTag);
                lastIndex = token.TokenPosition + token.TokenType.Template.Length;
            }

            return htmlBuilder.ToString();
        }

        private List<SingleToken> GetSortedPositionsWithTags
            (Dictionary<TokenType, List<TokenPosition>> positionsForTokensTypes)
        {
            var sortedPositionsWithTags = new List<SingleToken>();
            foreach (var tokenWithPositions in positionsForTokensTypes)
            foreach (var position in tokenWithPositions.Value)
            {
                sortedPositionsWithTags.Add
                    (new SingleToken(tokenWithPositions.Key, position.Start, LocationType.Opening));

                sortedPositionsWithTags.Add
                    (new SingleToken(tokenWithPositions.Key, position.End, LocationType.Closing));
            }

            return sortedPositionsWithTags;
        }
    }
}
