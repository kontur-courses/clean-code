using System;

namespace Markdown.Extensions
{
    internal static class IntExtension
    {
        public static bool IsBetween(this int source, int first, int second)
        {
            var min = Math.Min(first, second);
            var max = Math.Max(first, second);

            return min <= source && source <= max;
        }
    }
}