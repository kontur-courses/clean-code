using System.Collections.Generic;

namespace Markdown
{
    public static class ListExtensions
    {
        public static T LastOrDefault<T>(this List<T> list) => list.Count > 0 ? list[list.Count - 1] : default(T);

        public static LinkedList<T> ToLinkedList<T>(this List<T> list)
        {
            var result = new LinkedList<T>();
            foreach (var element in list)
            {
                result.AddLast(element);
            }

            return result;
        }
    }
   
}
