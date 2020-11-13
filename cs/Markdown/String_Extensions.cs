namespace Markdown
{
    public static class String_Extensions
    {
        public static bool IsLetterInPosition(this string text, int position)
            => InBounds(text, position) && char.IsLetter(text[position]);

        public static bool IsWhiteSpaceInPosition(this string text, int position)
            => InBounds(text, position) && char.IsWhiteSpace(text[position]);

        public static bool IsDigitInPosition(this string text, int position)
            => InBounds(text, position) && char.IsDigit(text[position]);

        public static bool IsSlashInPosition(this string text, int position)
            => InBounds(text, position) && text[position] == '\\';

        private static bool InBounds(string text, int position) => position >= 0 && position < text.Length;
    }
}