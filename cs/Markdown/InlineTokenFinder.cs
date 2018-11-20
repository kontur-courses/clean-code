using System.Collections.Generic;

namespace Markdown
{
    public class InlineTokenFinder
    {
        private readonly List<TokenType> tokensTypes = new List<TokenType>
        {
            new TokenType(TokenTypeEnum.SingleUnderscore, "_", "em", TokenLocationType.InlineToken),
            new TokenType(TokenTypeEnum.DoubleUnderscore, "__", "strong", TokenLocationType.InlineToken)
        };

        public List<SingleToken> FindInlineTokensInMdText(string paragraph)
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