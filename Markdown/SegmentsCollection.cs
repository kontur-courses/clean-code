using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class SegmentsCollection
    {
        private readonly IEnumerable<TokenSegment> segments;

        internal SegmentsCollection(IEnumerable<TokenSegment> segments)
        {
            this.segments = segments;
        }

        internal IEnumerable<TokenSegment> GetSortedSegments()
        {
            return segments.OrderBy(x => x.StartPosition);
        }
    }
}