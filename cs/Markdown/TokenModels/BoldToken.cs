using System;
using Markdown.Core;

namespace Markdown.TokenModels
{
    public class BoldToken : IToken
    {
        public static string MdTag => "__";

        public int MdTokenLength { get; }
        public string ToHtmlString() => $"<strong>{Children.ToHtmlString()}</strong>";
        private StringToken Children { get; }

        private BoldToken(StringToken children, int rawTokenLength)
        {
            Children = children;
            MdTokenLength = MdTag.Length + rawTokenLength + MdTag.Length;
        }

        public static BoldToken Create(string mdString, int startIndex)
        {
            var endIndex = GetTokenEndIndex(mdString, startIndex);
            var rawToken = mdString.Substring(startIndex + MdTag.Length, endIndex - startIndex - MdTag.Length);
            var rawStringToken = HtmlConverter.ConvertToHtmlString(rawToken);
            return new BoldToken(StringToken.Create(rawStringToken), rawToken.Length);
        }

        private static int GetTokenEndIndex(string mdString, int startIndex)
        {
            var analyzer = new StringAnalyzer(mdString);
            var endIndex = startIndex + MdTag.Length;
            var hasIntersectionWithItalicTag = false;

            while (analyzer.IsCharInsideValue(endIndex + 1) && !AreDoubleUnderscore(analyzer, endIndex))
            {
                endIndex++;
                if (analyzer.HasValueUnderscoreAt(endIndex) && !analyzer.HasValueUnderscoreAt(endIndex + 1))
                    hasIntersectionWithItalicTag = !hasIntersectionWithItalicTag;
            }

            ThrowArgumentExceptionIsIncorrectBoldToken(analyzer, startIndex, endIndex, hasIntersectionWithItalicTag);
            
            return endIndex;
        }


        private static void ThrowArgumentExceptionIsIncorrectBoldToken(
            StringAnalyzer analyzer,
            int startIndex,
            int endIndex,
            bool hasUnderscoreWithItalicTag
        )
        {
            if (endIndex - startIndex <= 2)
                throw new ArgumentException($"{nameof(BoldToken)} should has length more than 0!");

            if (!analyzer.HasValueUnderscoreAt(startIndex + 1) || !analyzer.HasValueUnderscoreAt(endIndex + 1))
                throw new ArgumentException(
                    $"{nameof(BoldToken)} should starts and ends with double underscore!");

            if (analyzer.HasValueWhiteSpaceAt(endIndex - 1))
                throw new ArgumentException($"{nameof(BoldToken)} shouldn't has white space before close tag!");

            if (hasUnderscoreWithItalicTag)
            {
                var message =
                    $"{nameof(BoldToken)} should ends with double underscore and shouldn't has between open and close tags unpaired single underscore";
                throw new ArgumentException(message);
            }

            if (analyzer.HasValueSelectionPartWordInDifferentWords(startIndex, endIndex))
                throw new ArgumentException(
                    $"{nameof(BoldToken)} must highlight either part of a single word, or the entire word!"
                );
        }

        private static bool AreDoubleUnderscore(StringAnalyzer analyzer, int endIndex) =>
            analyzer.HasValueUnderscoreAt(endIndex) && analyzer.HasValueUnderscoreAt(endIndex + 1);
    }
}