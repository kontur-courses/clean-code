using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class TokenFinder
    {
        private static readonly List<MarkdownToken> MarkdownTokens = new List<MarkdownToken>
        {
            new MarkdownToken("simpleUnderscore", "_", "em"),
            new MarkdownToken("doubleUnderscore", "__", "strong")
        };

        private void FindOpeningAndClosingTemplates(string paragraph,
            Dictionary<MarkdownToken, List<int>> openingPositionsForTokens, 
            Dictionary<MarkdownToken, List<int>> closingPositionsForTokens)
        {
            for (var index = 0; index < paragraph.Length; index++)
            {
                var openingToken = MarkdownTokens.GetOpeningToken(paragraph, index);
                var closingToken = MarkdownTokens.GetClosingToken(paragraph, index);

                if (openingToken != null)
                {
                    if (!openingPositionsForTokens.ContainsKey(openingToken))
                        openingPositionsForTokens[openingToken] = new List<int>();
                    openingPositionsForTokens[openingToken].Add(index);
                }
                if (closingToken != null)
                {
                    if (!closingPositionsForTokens.ContainsKey(closingToken))
                        closingPositionsForTokens[closingToken] = new List<int>();
                    closingPositionsForTokens[closingToken].Add(index);
                }
            }
        }

        private Dictionary<MarkdownToken, List<TokenPosition>> GetTokensBoarders(
            Dictionary<MarkdownToken, List<int>> openingPositionsForTokens, 
            Dictionary<MarkdownToken, List<int>> closingPositionsForTokens)
        {
            var tokensBoarders = new Dictionary<MarkdownToken, List<TokenPosition>>();
            foreach (var openingPositionsForToken in openingPositionsForTokens)
            {
                var token = openingPositionsForToken.Key;
                if (!closingPositionsForTokens.ContainsKey(token)) continue;

                tokensBoarders.Add(token, GetPositionsForToken(token, openingPositionsForTokens, closingPositionsForTokens));
            }

            return tokensBoarders;
        }

        private List<TokenPosition> GetPositionsForToken(
            MarkdownToken markdownToken,
            Dictionary<MarkdownToken, List<int>> openingPositionsForTokens, 
            Dictionary<MarkdownToken, List<int>> closingPositionsForTokens)
        {
            var usedPositions = new HashSet<int>();

            var positionsForTokens = new List<TokenPosition>();

            var openingPositions = new List<int>(openingPositionsForTokens[markdownToken]);
            var closingPositions = new List<int>(closingPositionsForTokens[markdownToken]);
            openingPositions.Reverse();

            foreach (var openingPosition in openingPositions)
            {
                if (usedPositions.Contains(openingPosition))
                    continue;
                var closingPosition = closingPositions
                    .FirstOrDefault(
                        position =>
                            position > openingPosition &&
                            !usedPositions.Contains(position));
                if (closingPosition == 0)
                    continue;

                positionsForTokens.Add(new TokenPosition(openingPosition, closingPosition));
                usedPositions.Add(openingPosition);
                usedPositions.Add(closingPosition);
            }

            return positionsForTokens;
        }

        public Dictionary<MarkdownToken, List<TokenPosition>> GetTokensWithPositions(string paragraph)
        {
            var openingPositionsForTokens = new Dictionary<MarkdownToken, List<int>>();
            var closingPositionsForTokens = new Dictionary<MarkdownToken, List<int>>();
            FindOpeningAndClosingTemplates(paragraph, openingPositionsForTokens, closingPositionsForTokens);

            return GetTokensBoarders(openingPositionsForTokens, closingPositionsForTokens);
        }
    }
}
