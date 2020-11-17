using System.Collections.Generic;
using System.Linq;
using Markdown.Core;

namespace Markdown.TokenModels
{
    public class ItalicToken : IToken
    {
        private IEnumerable<IToken> Children { get; }
        public int MdTokenLength => 1 + Children.Sum(t => t.MdTokenLength) + 1;

        private ItalicToken(IEnumerable<IToken> children) => Children = children;

        public string ToHtmlString() => $"<em>{Children.ConvertToHtmlString()}</em>";

        public static ItalicToken Create(string mdString, int startIndex)
        {
            if (!mdString.HasUnderscoreAt(startIndex) || !TryGetEndOfToken(mdString, startIndex + 1, out var endIndex)
                                                      || DoesntPassValidation(mdString, startIndex, endIndex))
                return null;

            var rawToken = mdString
                .Substring(startIndex + 1, endIndex - startIndex - 1)
                .Replace("__", @"\_\_");
            return new ItalicToken(Tokenizer.ParseIntoTokens(rawToken));
        }

        private static bool TryGetEndOfToken(string mdString, int startIndex, out int endIndex)
        {
            endIndex = startIndex;
            var hasIntersectionWithBoldTag = false;

            while (mdString.IsCharInsideString(endIndex) && !mdString.HasUnderscoreAt(endIndex))
            {
                ++endIndex;
                if (mdString.HasUnderscoreAt(endIndex) && mdString.HasUnderscoreAt(endIndex + 1))
                {
                    endIndex += 2;
                    hasIntersectionWithBoldTag = !hasIntersectionWithBoldTag;
                }
            }

            return !hasIntersectionWithBoldTag && mdString.HasUnderscoreAt(endIndex);
        }

        private static bool DoesntPassValidation(string mdString, in int startIndex, in int endIndex)
        {
            var hasDigitAroundTags = IsDigitAroundTag(mdString, startIndex) || IsDigitAroundTag(mdString, endIndex);
            var hasSpaceCharBeforeClosingTag = mdString.HasWhiteSpaceAt(endIndex - 1);
            return hasDigitAroundTags || hasSpaceCharBeforeClosingTag;
        }

        private static bool IsDigitAroundTag(string mdString, int position) =>
            mdString.HasDigitAt(position - 1) || mdString.HasDigitAt(position + 1);
    }
}