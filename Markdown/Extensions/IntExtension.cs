using System;

namespace Markdown.Extensions
{
    public static class IntExtension
    {
        public static bool Between(this int source, int left, int right)
        {
            var min = Math.Min(left, right);
            var max = Math.Max(left, right);

            return min <= source && source <= max;
        }
    }
}