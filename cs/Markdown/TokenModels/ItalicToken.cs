using System;
using Markdown.Core;

namespace Markdown.TokenModels
{
    public class ItalicToken : IToken
    {
        public static string MdTag => "_";

        public int MdTokenLength { get; }
        private StringToken Children { get; }

        private ItalicToken(StringToken children, int rawTokenLength)
        {
            Children = children;
            MdTokenLength = MdTag.Length + rawTokenLength + MdTag.Length;
        }

        public string ToHtmlString() => $"<em>{Children.ToHtmlString()}</em>";

        public static ItalicToken Create(string mdString, int startIndex)
        {
            var analyzer = new StringAnalyzer(mdString);
            var endIndex = GetEndOfToken(analyzer, startIndex, out var hasIntersectionWithBoldTag);

            ThrowArgumentExceptionIfTokenIncorrect(analyzer, startIndex, endIndex, hasIntersectionWithBoldTag);

            var rawToken = mdString
                .Substring(startIndex + MdTag.Length, endIndex - startIndex - MdTag.Length)
                .Replace("__", @"\_\_");
            var rawStringToken = HtmlConverter.ConvertToHtmlString(rawToken);
            return new ItalicToken(StringToken.Create(rawStringToken), rawStringToken.Length);
        }

        private static void ThrowArgumentExceptionIfTokenIncorrect(
            StringAnalyzer analyzer,
            int startIndex,
            int endIndex,
            bool hasIntersectionWithBoldTag)
        {
            if (endIndex - startIndex <= 1)
                throw new ArgumentException($"{nameof(ItalicToken)} should has length more than 0!");

            if (!analyzer.HasValueUnderscoreAt(startIndex) || !analyzer.HasValueUnderscoreAt(endIndex))
                throw new ArgumentException($"{nameof(ItalicToken)} should starts and ends with underscore!");

            if (analyzer.HasValueWhiteSpaceAt(endIndex - 1))
                throw new ArgumentException($"{nameof(ItalicToken)} shouldn't has white space before close tag!");

            if (hasIntersectionWithBoldTag)
                throw new ArgumentException(
                    $"{nameof(ItalicToken)} should ends with underscore and shouldn't has between open and close tags unpaired double underscore"
                );

            if (IsDigitAroundItalicTag(analyzer, startIndex) || IsDigitAroundItalicTag(analyzer, endIndex))
                throw new ArgumentException($"{nameof(ItalicToken)} shouldn't has digits around open and close tags!");

            if (analyzer.HasValueSelectionPartWordInDifferentWords(startIndex, endIndex))
                throw new ArgumentException(
                    $"{nameof(ItalicToken)} must highlight either part of a single word, or the entire word!"
                );
        }

        private static int GetEndOfToken(StringAnalyzer analyzer, int startIndex, out bool hasIntersectionWithBoldTag)
        {
            var endIndex = startIndex + MdTag.Length;
            hasIntersectionWithBoldTag = false;

            while (analyzer.IsCharInsideValue(endIndex) && !analyzer.HasValueUnderscoreAt(endIndex))
            {
                endIndex++;
                if (analyzer.HasValueUnderscoreAt(endIndex) && analyzer.HasValueUnderscoreAt(endIndex + 1))
                {
                    endIndex += 2;
                    hasIntersectionWithBoldTag = !hasIntersectionWithBoldTag;
                }
            }

            return endIndex;
        }

        private static bool IsDigitAroundItalicTag(StringAnalyzer analyzer, int position)
        {
            var hasDigitBefore = analyzer.IsCharInsideValue(position - 1) &&
                                 char.IsDigit(analyzer.AnalyzedString[position - 1]);
            var hasDigitAfter = analyzer.IsCharInsideValue(position + 1) &&
                                char.IsDigit(analyzer.AnalyzedString[position + 1]);
            return hasDigitBefore || hasDigitAfter;
        }
    }
}