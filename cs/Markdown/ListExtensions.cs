using System.Collections.Generic;

namespace Markdown
{
    public static class ListExtensions
    {
        public static T LastOrDefault<T>(this List<T> list) => list.Count > 0 ? list[list.Count - 1] : default(T);
    }
}