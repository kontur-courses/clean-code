using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    internal class TokenInfoCollection
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
}