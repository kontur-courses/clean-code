using System;

namespace Markdown
{
    public static class ReadOnlySpanExtensions
    {
        public static bool Any<T>(this ReadOnlySpan<T> span, Func<T, bool> checker)
        {
            foreach (var e in span)
            {
                if (checker(e))
                    return true;
            }

            return false;
        }
    }
}