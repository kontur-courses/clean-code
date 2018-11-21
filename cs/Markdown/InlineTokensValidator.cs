using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class InlineTokensValidator
    {
        public List<SingleToken> GetValidTokens(List<SingleToken> tokens)
        {
            var validTokens = new List<SingleToken>();
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
                    validTokens.Add(token);
                    validTokens.Add(notClosedTokens[lastIndex]);
                    notClosedTokens.RemoveRange(lastIndex, notClosedTokens.Count - lastIndex);
                }
                else
                {
                    throw new InvalidOperationException("Invalid location type");
                }
            }

            return validTokens;
        }
    }
}
