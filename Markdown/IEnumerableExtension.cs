using System;
using System.Collections.Generic;

namespace Markdown
{
    public static class EnumerableExtension
    {
        internal static IEnumerable<TSource> ForEachPairs<TSource>(
            this IEnumerable<TSource> sources,
            Func<(TSource, TSource), (TSource, TSource)> action)
        {
            throw new NotImplementedException();
        }
    }
}