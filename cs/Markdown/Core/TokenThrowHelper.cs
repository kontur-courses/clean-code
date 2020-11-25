using System;
using Markdown.TokenModels;

namespace Markdown.Core
{
    public static class TokenThrowHelper
    {
        public static void AssertThatExtractedItalicTokenCorrect(
            StringAnalyzer analyzer,
            int startIndex,
            int endIndex,
            bool hasUnderscoreWithBoldTag
        )
        {
            if (endIndex - startIndex <= 1)
                throw new ArgumentException($"{nameof(ItalicToken)} should has length more than 0!");

            if (!analyzer.HasValueUnderscoreAt(startIndex) || !analyzer.HasValueUnderscoreAt(endIndex))
                throw new ArgumentException($"{nameof(ItalicToken)} should starts and ends with underscore!");

            if (analyzer.HasValueWhiteSpaceAt(endIndex - 1))
                throw new ArgumentException($"{nameof(ItalicToken)} shouldn't has white space before close tag!");

            if (hasUnderscoreWithBoldTag)
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

        public static void AssertThatExtractedBoldTokenCorrect(
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

        public static void AssertDescriptionIsCorrect(string mdString, int startIndex, int descriptionEndIndex)
        {
            var analyzer = new StringAnalyzer(mdString);
            if (mdString[startIndex] != '[')
                throw new ArgumentException($"{nameof(LinkToken)} should starts with open square bracket!");

            if (!analyzer.IsCharInsideValue(descriptionEndIndex) || mdString[descriptionEndIndex] != ']')
                throw new ArgumentException($"{nameof(LinkToken)} should ends with end square bracket!");

            if (descriptionEndIndex - startIndex <= 1)
                throw new ArgumentException($"Description in {nameof(LinkToken)} should has length more than 0!");

            if (analyzer.IsCharInsideValue(descriptionEndIndex + 1) && mdString[descriptionEndIndex + 1] != '(')
                throw new ArgumentException(
                    $"{nameof(LinkToken)} should has opening parenthesis after closing square bracket!"
                );
        }

        public static void AssertLinkIsCorrect(string mdString, int linkEndIndex, int descriptionEndIndex)
        {
            var analyzer = new StringAnalyzer(mdString);
            if (!analyzer.IsCharInsideValue(linkEndIndex) || mdString[linkEndIndex] != ')')
                throw new ArgumentException($"{nameof(LinkToken)} should ends with end parenthesis!");

            if (linkEndIndex - (descriptionEndIndex + 1) <= 1)
                throw new ArgumentException($"Link in {nameof(LinkToken)} should has length more than 0!");
        }

        private static bool IsDigitAroundItalicTag(StringAnalyzer analyzer, int position)
        {
            var hasDigitBefore = analyzer.IsCharInsideValue(position - 1) && char.IsDigit(analyzer.AnalyzedString[position - 1]);
            var hasDigitAfter = analyzer.IsCharInsideValue(position + 1) && char.IsDigit(analyzer.AnalyzedString[position + 1]);
            return hasDigitBefore || hasDigitAfter;
        }
    }
}