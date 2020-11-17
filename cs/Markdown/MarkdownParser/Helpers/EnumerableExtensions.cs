using System;
using System.Collections.Generic;
using System.Linq;

namespace MarkdownParser.Helpers
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> WhereNot<T>(this IEnumerable<T> source, Func<T, bool> predicate) =>
            source.Where(x => !predicate.Invoke(x));

        public static T WithMax<T, TComparable>(this IEnumerable<T> source, Func<T, TComparable> comparingSelector)
            where TComparable : IComparable<TComparable> =>
            source.OrderByDescending(comparingSelector).FirstOrDefault();
    }
}