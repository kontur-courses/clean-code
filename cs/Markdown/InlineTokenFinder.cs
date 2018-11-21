using System.Collections.Generic;

namespace Markdown
{
    public class InlineTokenFinder
    {
        public List<SingleToken> ExtractInlineTokensFromText(string paragraph, IEnumerable<TokenType> tokensTypes)
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

        public List<SingleToken> FindInlineTokens(string paragraph, IEnumerable<TokenType> tokensTypes)
        {
            var inlineTokens = ExtractInlineTokensFromText(paragraph, tokensTypes);
            var validInlineTokens = new InlineTokensValidator().GetValidTokens(inlineTokens);

            return validInlineTokens;
        }
    }
}