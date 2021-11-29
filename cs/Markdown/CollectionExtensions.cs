using System.Collections;

namespace Markdown
{
    public static class CollectionExtensions
    {
        public static bool InBorders(this ICollection collection, int position)
        {
            return 0 <= position && position < collection.Count;
        }
    }
}