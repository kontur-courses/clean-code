using System.Collections.Generic;
using System.Linq;
using Markdown.Core;

namespace Markdown.TokenModels
{
    public class BoldToken : IToken
    {
        private IEnumerable<IToken> Children { get; }
        public int MdTokenLength => Children.Sum(t => t.MdTokenLength) + 4;
        public string ToHtmlString() => $"<strong>{Children.ConvertToHtmlString()}</strong>";

        private BoldToken(IEnumerable<IToken> children) => Children = children;

        public static BoldToken Create(string mdString, int startIndex)
        {
            if (!mdString.HasUnderscoreAt(startIndex) || !TryGetEndOfToken(mdString, startIndex + 2, out var endIndex)
                                                      || DoesntPassValidation(mdString, startIndex, endIndex))
                return default;

            var rawToken = mdString.Substring(startIndex + 2, endIndex - startIndex - 2);
            return new BoldToken(Tokenizer.ParseIntoTokens(rawToken));
        }

        private static bool TryGetEndOfToken(string mdString, int startIndex, out int endIndex)
        {
            endIndex = startIndex;
            var hasIntersectionWithItalicTag = false;
            //f__oo ba__r
            while (mdString.IsCharInsideString(endIndex + 1) && !AreDoubleUnderscore(mdString, endIndex))
            {
                endIndex++;
                if (mdString.HasUnderscoreAt(endIndex) && !mdString.HasUnderscoreAt(endIndex + 1))
                    hasIntersectionWithItalicTag = !hasIntersectionWithItalicTag;
            }

            return !hasIntersectionWithItalicTag && mdString.HasUnderscoreAt(endIndex + 1);
        }

        private static bool DoesntPassValidation(string mdString, int startIndex, int endIndex) =>
            endIndex - startIndex <= 2 || mdString.HasWhiteSpaceAt(endIndex - 1) ||
            mdString.HasSelectionPartWordInDifferentWords(startIndex, endIndex);

        private static bool AreDoubleUnderscore(string mdString, int endIndex) =>
            mdString.HasUnderscoreAt(endIndex) && mdString.HasUnderscoreAt(endIndex + 1);
    }
}