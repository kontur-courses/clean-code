using System;
using System.Linq;

namespace Markdown.Extensions
{
    public static class StringExtensions
    {
        public static char[] GetNeighborsOfSymbol(this string text, int position)
        {
            if (position >= text.Length || position < 0)
                throw new ArgumentException(
                    $"{nameof(position)} {position} is not in string with length {text.Length}");
            if (text.Length <= 1) return new char[0];
            if (position > 0 && position < text.Length - 1)
                return new[] {text[position - 1], text[position + 1]};
            return position == 0 ? new[] {text[position + 1]} : new[] {text[position - 1]};
        }

        public static char[] GetNeighborsOfSubstring(this string text, int startPosition, int endPosition)
        {
            if (startPosition == 0)
            {
                return new[] {GetNeighborsOfSymbol(text, endPosition).Last()};
            }

            if (endPosition == text.Length - 1)
            {
                return new[] {GetNeighborsOfSymbol(text, startPosition).First()};
            }

            return new[]
                {GetNeighborsOfSymbol(text, endPosition).Last(), GetNeighborsOfSymbol(text, startPosition).First()};
        }
    }
}