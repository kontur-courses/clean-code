using System.Collections.Generic;

namespace Markdown
{
    public class TokensValidator
    {
        public Dictionary<TokenType, List<TokenPosition>> GetPositionsForTokens(List<SingleToken> tokens)
        {
            var separatedTokens = GetTokensForTokenTypes(tokens);
            var tokensPositions = new Dictionary<TokenType, List<TokenPosition>>();

            foreach (var tokensForType in separatedTokens)
            {
                var token = tokensForType.Key;
                tokensPositions.Add(token, GetPositionsForTokenType(separatedTokens[token]));
            }

            return tokensPositions;
        }

        private Dictionary<TokenType, List<SingleToken>> GetTokensForTokenTypes(List<SingleToken> tokens)
        {
            var allTokens = new Dictionary<TokenType, List<SingleToken>>();

            foreach (var token in tokens)
            {
                if (!allTokens.ContainsKey(token.TokenType))
                    allTokens.Add(token.TokenType, new List<SingleToken>());
                allTokens[token.TokenType].Add(token);
            }

            return allTokens;
        }

        private List<TokenPosition> GetPositionsForTokenType(List<SingleToken> tokens)
        {
            var localTokens = new List<SingleToken>(tokens);
            var positions = new List<TokenPosition>();

            var index = 0;

            while (localTokens.Count > 0 && index < localTokens.Count)
            {
                if (localTokens[index].LocationType == LocationType.Closing)
                {
                    if (index > 0)
                    {
                        positions.Add(new TokenPosition(localTokens[index - 1].TokenPosition,
                            localTokens[index].TokenPosition));
                        localTokens.RemoveAt(index - 1);
                        index -= 1;
                    }
                    localTokens.RemoveAt(index);
                }
                else if (localTokens[index].LocationType == LocationType.Opening)
                {
                    index += 1;
                }
            }

            return positions;
        }
    }
}
