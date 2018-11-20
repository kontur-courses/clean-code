using System.Collections.Generic;

namespace Markdown
{
    public class TokenFinder
    {
        private readonly List<TokenType> tokensTypes = new List<TokenType>
        {
            new TokenType("simpleUnderscore", "_", "em"),
            new TokenType("doubleUnderscore", "__", "strong")
        };

        public List<SingleToken> FindTokensInMdText(string paragraph)
        {
            var tokens = new List<SingleToken>();

            for (var index = 0; index < paragraph.Length; index++)
            {
                var openingToken = tokensTypes.GetOpeningToken(paragraph, index);
                var closingToken = tokensTypes.GetClosingToken(paragraph, index);

                if (openingToken != null)
                    tokens.Add(new SingleToken(openingToken, index, LocationType.Opening));
                if (closingToken != null)
                    tokens.Add(new SingleToken(closingToken, index, LocationType.Closing));
            }

            return tokens;
        }
    }
}