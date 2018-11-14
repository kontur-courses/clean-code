using System.Collections.Generic;

namespace MarkDown
{
    public static class ListExtensions
    {
        public static void ConditionalAdd<T>(this List<T> enumerable, bool condition, params T[] toAdd)
        {
            if (condition)
                enumerable.AddRange(toAdd);
        }
    }
}
