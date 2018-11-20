using System.Collections.Generic;

namespace Markdown
{
    public class StartingTokenFinder
    {
        private readonly List<TokenType> tokensTypes = new List<TokenType>
        {
            new TokenType("lattice", "#", "h1", TokenLocationType.InlineToken),
            new TokenType("lattice", "##", "h2", TokenLocationType.InlineToken),
            new TokenType("lattice", "###", "h3", TokenLocationType.InlineToken),
            new TokenType("lattice", "####", "h4", TokenLocationType.InlineToken),
            new TokenType("star", "*", "li", TokenLocationType.InlineToken)
        };

        public List<SingleToken> FindStartingTokens(string paragraph)
        {
            var tokens = new List<SingleToken>();

            var currentIndex = 0;
            var usedTokens = new HashSet<TokenType>();

            foreach (var word in paragraph.Split())
            {
                if (word == string.Empty)
                {
                    currentIndex += 1;
                    continue;
                }

                var finded = false;
                foreach (var tokenType in tokensTypes)
                    if (!usedTokens.Contains(tokenType) && tokenType.Template == word)
                    {
                        tokens.Add(new SingleToken(tokenType, currentIndex, LocationType.Single));
                        usedTokens.Add(tokenType);
                        finded = true;
                        break;
                    }

                if (!finded)
                    return tokens;

                currentIndex += word.Length + 1;
            }

            return tokens;
        }
    }
}
