using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class StartingTokenFinder
    {
        private readonly List<TokenType> tokensTypes = new List<TokenType>
        {
            new TokenType("lattice", "#", "strong", TokenLocationType.InlineToken),
            new TokenType("star", "*", "li", TokenLocationType.InlineToken)
        };

        public List<SingleToken> FindStartingTokens(string paragraph)
        {
            var tokens = new List<SingleToken>();

            var currentIndex = 0;

            foreach (var word in paragraph.Split())
            {
                if (word == string.Empty)
                    currentIndex += 1;

                var finded = false;
                foreach (var tokenType in tokensTypes)
                    if (tokenType.Template == word)
                    {
                        tokens.Add(new SingleToken(tokenType, currentIndex, LocationType.Single));
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
