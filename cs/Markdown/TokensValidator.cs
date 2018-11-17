using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class TokensValidator
    {
        public Dictionary<TokenType, List<TokenPosition>> GetPositionsForTokens(
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
    }
}
