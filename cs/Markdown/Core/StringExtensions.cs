namespace Markdown.Core
{
    public static class StringExtensions
    {
        public static bool HasWhiteSpaceAt(this string str, int index) =>
            str.IsCharInsideString(index) && char.IsWhiteSpace(str[index]);

        public static bool HasDigitAt(this string str, int index) =>
            str.IsCharInsideString(index) && char.IsDigit(str[index]);

        public static bool HasUnderscoreAt(this string str, int index) =>
            str.IsCharInsideString(index) && str[index] == '_';

        public static bool IsCharInsideString(this string str, int index) => index >= 0 && index < str.Length;
    }
}