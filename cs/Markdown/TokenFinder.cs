using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class TokenFinder
    {
        private static readonly List<TokenType> TokensTypes = new List<TokenType>
        {
            new TokenType("simpleUnderscore", "_", "em"),
            new TokenType("doubleUnderscore", "__", "strong")
        };

        private void FindOpeningAndClosingTemplates(string paragraph,
            Dictionary<TokenType, List<int>> openingPositionsForTokens, 
            Dictionary<TokenType, List<int>> closingPositionsForTokens)
        {
            for (var index = 0; index < paragraph.Length; index++)
            {
                var openingToken = TokensTypes.GetOpeningToken(paragraph, index);
                var closingToken = TokensTypes.GetClosingToken(paragraph, index);

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

        private Dictionary<TokenType, List<TokenPosition>> GetTokensBoarders(
            Dictionary<TokenType, List<int>> openingPositionsForTokens, 
            Dictionary<TokenType, List<int>> closingPositionsForTokens)
        {
            var tokensBoarders = new Dictionary<TokenType, List<TokenPosition>>();
            foreach (var openingPositionsForToken in openingPositionsForTokens)
            {
                var token = openingPositionsForToken.Key;
                if (!closingPositionsForTokens.ContainsKey(token)) continue;

                tokensBoarders.Add(token, GetPositionsForToken(token, openingPositionsForTokens, closingPositionsForTokens));
            }

            return tokensBoarders;
        }

        private List<TokenPosition> GetPositionsForToken(
            TokenType tokenType,
            Dictionary<TokenType, List<int>> openingPositionsForTokens, 
            Dictionary<TokenType, List<int>> closingPositionsForTokens)
        {
            var usedPositions = new HashSet<int>();

            var positionsForTokens = new List<TokenPosition>();

            var openingPositions = new List<int>(openingPositionsForTokens[tokenType]);
            var closingPositions = new List<int>(closingPositionsForTokens[tokenType]);
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

        public Dictionary<TokenType, List<TokenPosition>> GetTokensWithPositions(string paragraph)
        {
            var openingPositionsForTokens = new Dictionary<TokenType, List<int>>();
            var closingPositionsForTokens = new Dictionary<TokenType, List<int>>();
            FindOpeningAndClosingTemplates(paragraph, openingPositionsForTokens, closingPositionsForTokens);

            return GetTokensBoarders(openingPositionsForTokens, closingPositionsForTokens);
        }
    }
}
