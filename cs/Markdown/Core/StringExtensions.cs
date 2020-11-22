using System;

namespace Markdown.Core
{
    public static class StringExtensions
    {
        public static bool HasWhiteSpaceAt(this string str, int index) =>
            str.IsCharInsideString(index) && char.IsWhiteSpace(str[index]);

        public static bool HasNonWhiteSpaceAt(this string str, int index) =>
            str.IsCharInsideString(index) && !char.IsWhiteSpace(str[index]);

        public static bool HasSelectionPartWordInDifferentWords(this string mdString, int startIndex, int endIndex) =>
            mdString.HasNonWhiteSpaceAt(startIndex - 1) && mdString.HasNonWhiteSpaceAt(endIndex + 1) &&
            mdString.IndexOf(" ", startIndex, endIndex - startIndex, StringComparison.Ordinal) != -1;

        public static bool HasDigitAt(this string str, int index) =>
            str.IsCharInsideString(index) && char.IsDigit(str[index]);

        public static bool HasUnderscoreAt(this string str, int index) =>
            str.IsCharInsideString(index) && str[index] == '_';

        public static bool IsCharInsideString(this string str, int index) => index >= 0 && index < str.Length;
    }
}