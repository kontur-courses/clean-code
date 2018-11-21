using System.Collections.Generic;

namespace Markdown
{
    public class StartingTokenFinder
    {
        private bool TryMatchWordToTokens(string word, int currentIndex, HashSet<TokenType> usedTokens, IEnumerable<TokenType> tokensTypes, out SingleToken token)
        {
            foreach (var tokenType in tokensTypes)
                if (!usedTokens.Contains(tokenType) && tokenType.Template == word)
                {
                    token = new SingleToken(tokenType, currentIndex, LocationType.Single);
                    return true;
                }

            token = null;
            return false;
        }

        public List<SingleToken> FindStartingTokens(string paragraph, IEnumerable<TokenType> tokensTypes)
        {
            var tokens = new List<SingleToken>();

            var currentIndex = 0;
            var usedTokensTypes = new HashSet<TokenType>();

            foreach (var word in paragraph.Split())
            {
                if (word == string.Empty)
                {
                    currentIndex += 1;
                    continue;
                }

                if (!TryMatchWordToTokens(word, currentIndex, usedTokensTypes, tokensTypes, out var token))
                    return tokens;
                tokens.Add(token);
                usedTokensTypes.Add(token.TokenType);

                currentIndex += word.Length + 1;
            }

            return tokens;
        }
    }
}
