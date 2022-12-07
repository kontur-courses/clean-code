using System;
using System.Collections.Generic;

namespace Markdown
{
    public static class IEnumerableExtension
    {
        public static IEnumerable<T> Clone<T>(this IEnumerable<T> enumerable) 
            where T : ICloneable
        {
            foreach (var item in enumerable)
                yield return (T)item.Clone();
        }
    }
}
