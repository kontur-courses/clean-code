using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    internal class SegmentsCollection
    {
        private readonly IEnumerable<TokenSegment> segments;

        internal SegmentsCollection(IEnumerable<TokenSegment> segments)
        {
            MdExceptionHelper.ThrowArgumentNullExceptionIf(new ExceptionCheckObject(nameof(segments), segments));
            this.segments = segments;
        }

        internal IEnumerable<TokenSegment> GetSortedSegments()
        {
            return segments.OrderBy(x => x.StartPosition);
        }
    }
}