using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown.Extentions
{
    public static class ListExtentions
    {
        public static IEnumerable<(T Previous, T Current)> GetBigrams<T>(this List<T> list)
        {
            if (list.Count == 0)
                yield break;
            for (int i = 0; i < list.Count - 1; i++)
                yield return ValueTuple.Create(list[i], list[i + 1]);
            yield return ValueTuple.Create(list.Last(), default(T));
        }
    }
}
