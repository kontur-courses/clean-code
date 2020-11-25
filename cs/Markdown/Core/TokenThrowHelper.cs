using System;
using Markdown.TokenModels;

namespace Markdown.Core
{
    public static class TokenThrowHelper
    {
        public static void AssertThatExtractedItalicTokenCorrect(
            string mdString,
            int startIndex,
            int endIndex,
            bool hasUnderscoreWithBoldTag
        )
        {
            if (endIndex - startIndex <= 1)
                throw new ArgumentException($"{nameof(ItalicToken)} should has length more than 0!");

            if (!mdString.HasUnderscoreAt(startIndex) || !mdString.HasUnderscoreAt(endIndex))
                throw new ArgumentException($"{nameof(ItalicToken)} should starts and ends with underscore!");

            if (mdString.HasWhiteSpaceAt(endIndex - 1))
                throw new ArgumentException($"{nameof(ItalicToken)} shouldn't has white space before close tag!");

            if (hasUnderscoreWithBoldTag)
                throw new ArgumentException(
                    $"{nameof(ItalicToken)} should ends with underscore and shouldn't has between open and close tags unpaired double underscore"
                );

            if (IsDigitAroundItalicTag(mdString, startIndex) || IsDigitAroundItalicTag(mdString, endIndex))
                throw new ArgumentException($"{nameof(ItalicToken)} shouldn't has digits around open and close tags!");

            if (mdString.HasSelectionPartWordInDifferentWords(startIndex, endIndex))
                throw new ArgumentException(
                    $"{nameof(ItalicToken)} must highlight either part of a single word, or the entire word!"
                );
        }

        public static void AssertThatExtractedBoldTokenCorrect(
            string mdString,
            int startIndex,
            int endIndex,
            bool hasUnderscoreWithItalicTag
        )
        {
            if (endIndex - startIndex <= 2)
                throw new ArgumentException($"{nameof(BoldToken)} should has length more than 0!");

            if (!mdString.HasUnderscoreAt(startIndex + 1) || !mdString.HasUnderscoreAt(endIndex + 1))
                throw new ArgumentException($"{nameof(BoldToken)} should starts and ends with double underscore!");

            if (mdString.HasWhiteSpaceAt(endIndex - 1))
                throw new ArgumentException($"{nameof(BoldToken)} shouldn't has white space before close tag!");

            if (hasUnderscoreWithItalicTag)
                throw new ArgumentException(
                    $"{nameof(BoldToken)} should ends with double underscore and shouldn't has between open and close tags unpaired single underscore"
                );

            if (mdString.HasSelectionPartWordInDifferentWords(startIndex, endIndex))
                throw new ArgumentException(
                    $"{nameof(BoldToken)} must highlight either part of a single word, or the entire word!"
                );
        }

        public static void AssertDescriptionIsCorrect(string mdString, int startIndex, int descriptionEndIndex)
        {
            if (mdString[startIndex] != '[')
                throw new ArgumentException($"{nameof(LinkToken)} should starts with open square bracket!");

            if (!mdString.IsCharInsideString(descriptionEndIndex) || mdString[descriptionEndIndex] != ']')
                throw new ArgumentException($"{nameof(LinkToken)} should ends with end square bracket!");

            if (descriptionEndIndex - startIndex <= 1)
                throw new ArgumentException($"Description in {nameof(LinkToken)} should has length more than 0!");

            if (mdString.IsCharInsideString(descriptionEndIndex + 1) && mdString[descriptionEndIndex + 1] != '(')
                throw new ArgumentException(
                    $"{nameof(LinkToken)} should has opening parenthesis after closing square bracket!"
                );
        }

        public static void AssertLinkIsCorrect(string mdString, int linkEndIndex, int descriptionEndIndex)
        {
            if (!mdString.IsCharInsideString(linkEndIndex) || mdString[linkEndIndex] != ')')
                throw new ArgumentException($"{nameof(LinkToken)} should ends with end parenthesis!");

            if (linkEndIndex - (descriptionEndIndex + 1) <= 1)
                throw new ArgumentException($"Link in {nameof(LinkToken)} should has length more than 0!");
        }

        private static bool IsDigitAroundItalicTag(string mdString, int position) =>
            mdString.HasDigitAt(position - 1) || mdString.HasDigitAt(position + 1);
    }
}