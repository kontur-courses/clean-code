using System;
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

        private List<SingleToken> FindOpeningAndClosingTemplates(string paragraph)
        {
            var tokens = new List<SingleToken>();

            for (var index = 0; index < paragraph.Length; index++)
            {
                var openingToken = TokensTypes.GetOpeningToken(paragraph, index);
                var closingToken = TokensTypes.GetClosingToken(paragraph, index);

                if (openingToken != null)
                    tokens.Add(new SingleToken(openingToken, index, LocationType.Opening));
                if (closingToken != null)
                    tokens.Add(new SingleToken(closingToken, index, LocationType.Closing));
            }

            return tokens;
        }

        public List<SingleToken> GetTokensOpeningAndClosingPositions(string paragraph)
        {
            var tokens = FindOpeningAndClosingTemplates(paragraph);

            return tokens;
        }
    }
}