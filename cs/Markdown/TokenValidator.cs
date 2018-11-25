using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class TokenValidator
    {
        public IEnumerable<HtmlTag> ValidInlineTokens(IEnumerable<SingleToken> tokens)
        {
            var validHtmlTags = new List<HtmlTag>();
            var notClosedTokens = new List<SingleToken>();

            foreach (var token in tokens)
            {
                if (token.LocationType == LocationType.Opening)
                {
                    notClosedTokens.Add(token);
                }
                else if (token.LocationType == LocationType.Closing)
                {
                    var lastIndex = notClosedTokens.Select(t => t.TokenType)
                        .ToList()
                        .LastIndexOf(token.TokenType);
                    if (lastIndex < 0)
                        continue;

                    validHtmlTags.Add(new HtmlTag(token.TokenType.HtmlTag, token.TokenPosition));
                    validHtmlTags.Add(new HtmlTag(notClosedTokens[lastIndex].TokenType.HtmlTag, notClosedTokens[lastIndex].TokenPosition));
                    notClosedTokens.RemoveRange(lastIndex, notClosedTokens.Count - lastIndex);
                }
                else
                {
                    throw new InvalidOperationException("Invalid location type");
                }
            }

            return validHtmlTags;
        }

        public IEnumerable<SingleToken> ValidStartingTokens()
        {
            throw new NotImplementedException();
        }
    }
}
