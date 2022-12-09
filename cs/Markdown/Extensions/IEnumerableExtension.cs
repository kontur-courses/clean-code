using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown.Extensions
{
    public static class IEnumerableExtension
    {
        public static IEnumerable<T> Clone<T>(this IEnumerable<T> enumerable) 
            where T : ICloneable
        {
            return enumerable.Select(item => (T)item.Clone());
        }

        public static IEnumerable<T> SelectWhere<T>(this IEnumerable<T> enumerable,
            Func<T, T> selector, Func<T, bool> predicate)
        {
            return enumerable.Select(item => predicate(item) ? selector(item) : item);
        }
    }
}
