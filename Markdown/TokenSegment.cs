using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Extensions;

namespace Markdown
{
    internal class TokenSegment
    {
        private readonly Token openToken;
        private readonly int openTokenLocation;
        private readonly Token closeToken;
        private readonly int closeTokenLocation;

        public int StartPosition => openTokenLocation;
        public int EndPosition => closeTokenLocation;
        public int Length => closeTokenLocation - openTokenLocation;
        
        private TokenSegment(int openTokenLocation, int closeTokenLocation, Token openToken, Token closeToken)
        {
            if (openToken is null || closeToken is null) throw new ArgumentNullException();
            
            this.openToken = openToken;
            this.openTokenLocation = openTokenLocation;
            this.closeToken = closeToken;
            this.closeTokenLocation = closeTokenLocation;
        }
        
        public static IEnumerable<TokenSegment> GetTokensSegments(IEnumerable<TokenInfo> tokensByLocation)
        {
            if (tokensByLocation is null) throw new ArgumentNullException();
            
            (int, Token)? currentOpenToken = null;
            foreach (var (index, token, close, open, _, _) in tokensByLocation.OrderBy(x => x.Position))
            {
                if (currentOpenToken is null && open)
                    currentOpenToken = (index, token);
                else if (currentOpenToken is not null && close)
                {
                    yield return new TokenSegment(currentOpenToken.Value.Item1, index, currentOpenToken.Value.Item2, token);
                    currentOpenToken = null;
                }
            }
        }
        
        public Tag GetBaseTag()
        {
            return Tag.GetTagByChars(openToken.ToString());
        }

        public bool Contain(TokenSegment other)
        {
            if (other is null) throw new ArgumentNullException();

            var firstMin = Math.Min(openTokenLocation, closeTokenLocation);
            var firstMax = Math.Max(openTokenLocation, closeTokenLocation);
            var secondMin = Math.Min(other.openTokenLocation, other.closeTokenLocation);
            var secondMax = Math.Max(other.openTokenLocation, other.closeTokenLocation);

            return secondMin.Between(firstMin, firstMax) && secondMax.Between(firstMin, firstMax);
            // return firstMin.Between(secondMin, secondMax) && firstMax.Between(secondMin, secondMax);
            // // || secondMin.Between(firstMin, firstMax) && secondMax.Between(firstMin, firstMax);
        }
        
        public bool IsIntersectWith(TokenSegment other)
        {
            if (other is null) throw new ArgumentNullException();

            var firstMin = Math.Min(openTokenLocation, closeTokenLocation);
            var firstMax = Math.Max(openTokenLocation, closeTokenLocation);
            var secondMin = Math.Min(other.openTokenLocation, other.closeTokenLocation);
            var secondMax = Math.Max(other.openTokenLocation, other.closeTokenLocation);

            return firstMin.Between(secondMin, secondMax) && !firstMax.Between(secondMin, secondMax)
                   || !firstMin.Between(secondMin, secondMax) && firstMax.Between(secondMin, secondMax);
        }
    }
}