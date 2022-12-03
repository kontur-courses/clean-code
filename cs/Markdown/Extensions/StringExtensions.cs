using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown.Extensions
{
    public static class StringExtensions
    {
        public static bool IsEnd(this string text, int position) =>
            position == text.Length - 1;

        public static bool IsInside(this string text, int position) =>
            0 <= position && position < text.Length;

        public static bool IsLetter(this string text, int position) =>
            text.IsInside(position) && (char.IsLetter(text[position]) || text[position] == '.');
    }
}
