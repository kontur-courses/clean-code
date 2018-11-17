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
        public (Dictionary<TokenType, List<int>> OpeningPositions, Dictionary<TokenType, List<int>> ClosingPositions) GetTokensOpeningAndClosingPositions(string paragraph)
        {
            var openingPositionsForTokens = new Dictionary<TokenType, List<int>>();
            var closingPositionsForTokens = new Dictionary<TokenType, List<int>>();
            FindOpeningAndClosingTemplates(paragraph, openingPositionsForTokens, closingPositionsForTokens);

            return (openingPositionsForTokens, closingPositionsForTokens);
        }


    }
}
