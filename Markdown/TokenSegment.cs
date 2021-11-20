using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
    public class TokenSegment
    {
        private readonly Token openToken;
        private readonly int openTokenLocation;
        private readonly Token closeToken;
        private readonly int closeTokenLocation;

        public int Length => closeTokenLocation - openTokenLocation;
        
        private TokenSegment(int openTokenLocation, int closeTokenLocation, Token openToken, Token closeToken)
        {
            this.openToken = openToken;
            this.openTokenLocation = openTokenLocation;
            this.closeToken = closeToken;
            this.closeTokenLocation = closeTokenLocation;
        }
        
        public static IEnumerable<TokenSegment> GetTokensSegments(Dictionary<int, TokenInfo> tokensByLocation)
        {
            if (tokensByLocation is null) throw new ArgumentNullException();
            
            (int, Token)? currentOpenToken = null;
            
            foreach (var (index, token) in tokensByLocation.OrderBy(x => x.Key))
            {
                if (currentOpenToken is null && token.OpenValid)
                    currentOpenToken = (index, token.Token);
                else if (currentOpenToken is not null && token.CloseValid)
                {
                    yield return new TokenSegment(currentOpenToken.Value.Item1, index, currentOpenToken.Value.Item2, token.Token);
                    currentOpenToken = null;
                }
            }
        }
        
        public Token GetBaseToken()
        {
            return openToken;
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

    public static class IntExt
    {
        public static bool Between(this int source, int left, int right)
        {
            var min = Math.Min(left, right);
            var max = Math.Max(left, right);

            return min <= source && source <= max;
        }
    }
}