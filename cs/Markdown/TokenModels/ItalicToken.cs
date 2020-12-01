using System;
using Markdown.Core;

namespace Markdown.TokenModels
{
    public class ItalicToken : IToken
    {
        public static readonly string MdTag = TagsConfigReader.GetMdTagForTokenName(nameof(ItalicToken));
        public int MdTokenLength { get; }
        private StringToken Children { get; }

        public ItalicToken(string mdString, int startIndex)
        {
            if (!IsOpeningMarkdownTag(mdString, startIndex))
                throw new ArgumentException();

            var analyzer = new StringAnalyzer(mdString);
            var endIndex = GetEndOfToken(analyzer, startIndex, out var hasIntersectionWithBoldTag);

            ThrowArgumentExceptionIfTokenIncorrect(analyzer, startIndex, endIndex, hasIntersectionWithBoldTag);

            var rawToken = mdString
                .Substring(startIndex + MdTag.Length, endIndex - startIndex - MdTag.Length)
                .Replace("__", @"\_\_");
            var rawStringToken = HtmlConverter.ConvertToHtmlString(rawToken);

            Children = new StringToken(rawStringToken);
            MdTokenLength = MdTag.Length + rawStringToken.Length + MdTag.Length;
        }

        public string ToHtmlString() => $"<em>{Children.ToHtmlString()}</em>";

        private static bool IsOpeningMarkdownTag(string mdString, int index)
        {
            if (mdString[index].ToString() != MdTag)
                return false;

            var analyzer = new StringAnalyzer(mdString);
            var isRejectedBoldTag =
                analyzer.HasValueUnderscoreAt(index - 1) || analyzer.HasValueUnderscoreAt(index + 1);
            var hasWhiteSpaceAfterTag = analyzer.HasValueWhiteSpaceAt(index + MdTag.Length);
            return !isRejectedBoldTag && analyzer.IsCharInsideValue(index + MdTag.Length) && !hasWhiteSpaceAfterTag;
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