using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Markdown
{
    public class InlineTokensValidator
    {
        public List<SingleToken> GetPositionsForTokens(List<SingleToken> tokens)
        {
            var separatedTokens = GetTokensForTokenTypes(tokens);
            var tokensStream = new List<SingleToken>();

            foreach (var tokensForType in separatedTokens)
            {
                var token = tokensForType.Key;
                var tokensPositions = GetPositionsForTokenType(separatedTokens[token]);
                foreach (var tokenPosition in tokensPositions)
                {
                    tokensStream.Add(new SingleToken(tokensForType.Key, tokenPosition.Start, LocationType.Opening));
                    tokensStream.Add(new SingleToken(tokensForType.Key, tokenPosition.End, LocationType.Closing));
                }
            }

            return tokensStream;
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

        private IEnumerable<TokenPosition> GetPositionsForTokenType(IEnumerable<SingleToken> tokens)
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
