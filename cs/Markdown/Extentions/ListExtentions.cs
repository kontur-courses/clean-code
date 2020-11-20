using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown.Extentions
{
    public static class ListExtentions
    {
        public static IEnumerable<Tuple<T, T>> GetBigrams<T>(this List<T> list)
        {
            if (list.Count == 0)
                yield break;
            for (int i = 0; i < list.Count - 1; i++)
                yield return Tuple.Create(list[i], list[i + 1]);
            yield return Tuple.Create(list.Last(), default(T));
        }
    }
}
