using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class SegmentsCollection
    {
        private readonly IEnumerable<TokenSegment> segments;

        private static SegmentsCollection Empty => new();

        private SegmentsCollection()
        {
            segments = Enumerable.Empty<TokenSegment>();
        }

        internal SegmentsCollection(IEnumerable<TokenSegment> segments)
        {
            this.segments = segments;
        }

        internal IEnumerable<TokenSegment> GetSortedSegments()
        {
            return segments.OrderBy(x => x.StartPosition);
        }

        private static SegmentsCollection UnionNotEmptyCollections(IList<SegmentsCollection> collections)
        {
            return new SegmentsCollection(
                collections
                    .Select(x => x.segments)
                    .Where(x => x.Any())
                    .Aggregate((x, y) => x.Union(y)));
        }

        public static SegmentsCollection Union(IEnumerable<SegmentsCollection> collections)
        {
            if (collections is null) throw new ArgumentNullException();
            
            var fixedCollections = collections
                .Where(x => x.segments.Any())
                .ToList();
            
            return !fixedCollections.Any() 
                ? Empty 
                : UnionNotEmptyCollections(fixedCollections);
        }
    }
}