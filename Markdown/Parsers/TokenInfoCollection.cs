using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Extensions;

namespace Markdown
{
    public class TokenInfoCollection
    {
        private readonly IEnumerable<TokenInfo> tokens;

        internal TokenInfoCollection(IEnumerable<TokenInfo> tokens)
        {
            this.tokens = tokens;
        }
        
        public TokenInfoCollection SelectValid()
        {
            return new TokenInfoCollection(tokens.Where(x => x.Valid));
        }
        
        public IEnumerable<TokenInfoCollection> GroupBy<TGroup>(Func<TokenInfo, TGroup> groupFunc)
        {
            return tokens
                .GroupBy(groupFunc)
                .Select(x => new TokenInfoCollection(x));
        }

        public SegmentsCollection ToSegmentsCollection()
        {
            return new SegmentsCollection(TokenSegment.GetTokensSegments(tokens));
        }

        public IEnumerable<TokenInfo> GetTokenInfos()
        {
            return tokens;
        }
    }

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

        public static SegmentsCollection Union(IEnumerable<SegmentsCollection> collections)
        {
            return new SegmentsCollection(
                collections
                .Select(x => x.segments)
                .Where(x => x.Any())
                .Aggregate((x, y) => x.Union(y)));
        }
    }
}