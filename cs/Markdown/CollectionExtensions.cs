using System.Collections;
using System.Runtime.CompilerServices;

namespace Markdown
{
    public static class CollectionExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool InBorders(this ICollection collection, int position)
        {
            return 0 <= position && position < collection.Count;
        }
    }
}