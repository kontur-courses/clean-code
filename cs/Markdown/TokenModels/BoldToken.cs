using System;
using Markdown.Core;

namespace Markdown.TokenModels
{
    public class BoldToken : IToken
    {
        public static readonly string MdTag = TagsConfigReader.GetMdTagForTokenName(nameof(BoldToken));
        public int MdTokenLength { get; }
        private StringToken Children { get; }

        public BoldToken(string mdString, int startIndex)
        {
            if (!IsOpeningMarkdownTag(mdString, startIndex))
                throw new ArgumentException();

            var analyzer = new StringAnalyzer(mdString);
            var endIndex = GetTokenEndIndex(analyzer, startIndex, out var hasIntersectionWithItalicTag);

            ThrowArgumentExceptionIsIncorrectBoldToken(analyzer, startIndex, endIndex, hasIntersectionWithItalicTag);

            var rawToken = mdString.Substring(startIndex + MdTag.Length, endIndex - startIndex - MdTag.Length);
            var rawStringToken = HtmlConverter.ConvertToHtmlString(rawToken);

            Children = new StringToken(rawStringToken);
            MdTokenLength = MdTag.Length + rawToken.Length + MdTag.Length;
        }

        private int GetTokenEndIndex(
            StringAnalyzer analyzer,
            int startIndex,
            out bool hasIntersectionWithItalicTag)
        {
            var endIndex = startIndex + MdTag.Length;
            hasIntersectionWithItalicTag = false;

            while (analyzer.IsCharInsideValue(endIndex + 1) && !AreDoubleUnderscore(analyzer, endIndex))
            {
                endIndex++;
                if (analyzer.HasValueUnderscoreAt(endIndex) && !analyzer.HasValueUnderscoreAt(endIndex + 1))
                    hasIntersectionWithItalicTag = !hasIntersectionWithItalicTag;
            }

            return endIndex;
        }

        public string ToHtmlString() => $"<strong>{Children.ToHtmlString()}</strong>";

        private bool IsOpeningMarkdownTag(string mdString, in int index)
        {
            var analyzer = new StringAnalyzer(mdString);
            if (!AreDoubleUnderscore(analyzer, index))
                return false;

            var hasWhiteSpaceAfterOpenTag = analyzer.HasValueWhiteSpaceAt(index + MdTag.Length);
            return analyzer.IsCharInsideValue(index + MdTag.Length) && !hasWhiteSpaceAfterOpenTag;
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

            if (!AreDoubleUnderscore(analyzer, startIndex) || !AreDoubleUnderscore(analyzer, endIndex))
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