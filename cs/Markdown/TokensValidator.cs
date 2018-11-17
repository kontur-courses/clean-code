using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class TokensValidator
    {
        public Dictionary<TokenType, List<TokenPosition>> GetPositionsForTokens(List<SingleToken> tokens)
        {
            var separatedTokens = SeparateOpeningAndClosingTokens(tokens);
            var separatedOpeningTokens = separatedTokens.openingTokens;
            var separatedClosingTokens = separatedTokens.closingTokens;

            var tokensBoarders = new Dictionary<TokenType, List<TokenPosition>>();
            foreach (var openingTokens in separatedOpeningTokens)
            {
                var token = openingTokens.Key;
                if (separatedClosingTokens.TryGetValue(token, out var closingTokens))
                    tokensBoarders.Add(token, GetPositionsForToken(token, openingTokens.Value, closingTokens));
            }

            return tokensBoarders;
        }

        private (Dictionary<TokenType, List<SingleToken>> openingTokens, Dictionary<TokenType, List<SingleToken>> closingTokens) SeparateOpeningAndClosingTokens(List<SingleToken> tokens)
        {
            var openingsTokens = new Dictionary<TokenType, List<SingleToken>>();
            var closingTokens = new Dictionary<TokenType, List<SingleToken>>();

            foreach (var token in tokens)
            {
                switch (token.LocationType)
                {
                    case LocationType.Opening:
                        if (!openingsTokens.ContainsKey(token.TokenType))
                            openingsTokens.Add(token.TokenType, new List<SingleToken>());
                        openingsTokens[token.TokenType].Add(token);

                        break;
                    case LocationType.Closing:
                        if (!closingTokens.ContainsKey(token.TokenType))
                            closingTokens.Add(token.TokenType, new List<SingleToken>());
                        closingTokens[token.TokenType].Add(token);
                        break;
                    default:
                        throw new InvalidOperationException("Invalid token location type");
                }
            }

            return (openingsTokens, closingTokens);
        }

        private List<TokenPosition> GetPositionsForToken(
            TokenType tokenType,
            List<SingleToken> openingPositionsForTokens,
            List<SingleToken> closingPositionsForTokens)
        {
            var usedPositions = new HashSet<int>();

            var positionsForTokens = new List<TokenPosition>();

            var openingPositions = new List<int>(openingPositionsForTokens.Select(token=>token.TokenPosition));
            var closingPositions = new List<int>(closingPositionsForTokens.Select(token => token.TokenPosition));
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
