using System;

namespace Markdown.Extensions
{
    public static class StringExtensions
    {
        public static bool IsInside(this string text, int position) =>
            0 <= position && position < text.Length;

        public static bool IsWhiteSpaceIn(this string text, int position) =>
            text.IsInside(position) && char.IsWhiteSpace(text[position]);

        public static bool Is(this string text, Func<char, bool> condition, int position) =>
            text.IsInside(position) && condition(text[position]);
    }
}
