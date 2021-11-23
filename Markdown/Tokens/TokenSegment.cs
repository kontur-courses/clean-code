using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Extensions;

namespace Markdown
{
    internal class TokenSegment
    {
        private readonly int openTokenLocation;
        private readonly int closeTokenLocation;
        private Tag tag;

        public bool InTextSegment { get; private init; }
        public int StartPosition => openTokenLocation;
        public int EndPosition => closeTokenLocation;
        public int Length => closeTokenLocation - openTokenLocation;
        public int InnerLength => closeTokenLocation - (openTokenLocation + tag.Start.Length);
        
        private TokenSegment(Tag tag, int openTokenLocation, int closeTokenLocation)
        {
            if (tag is null) throw new ArgumentNullException();
            if (tag.End is null && openTokenLocation != closeTokenLocation)
                throw new AggregateException("single tag open and close in different location");

            this.tag = tag;
            this.openTokenLocation = openTokenLocation;
            this.closeTokenLocation = closeTokenLocation;
        }
        
        public static IEnumerable<TokenSegment> GetTokensSegments(IEnumerable<TokenInfo> tokensByLocation)
        {
            if (tokensByLocation is null) throw new ArgumentNullException();
            
            (int, TokenInfo)? currentOpenToken = null;
            foreach (var info in tokensByLocation.OrderBy(x => x.Position))
            {
                var (index, token, close, open, _, _) = info;
                var tag = Tag.GetTagByChars(token.ToString());
                if (tag.End is null)
                {
                    yield return new TokenSegment(tag, index, index);
                }
                else if (currentOpenToken is null && open)
                    currentOpenToken = (index, info);
                else if (currentOpenToken is not null && close)
                {
                    // yield return new TokenSegment(currentOpenToken.Value.Item1, index, currentOpenToken.Value.Item2, token);
                    yield return new TokenSegment(Tag.GetTagByChars(currentOpenToken.Value.Item2.Token.ToString()), currentOpenToken.Value.Item1, index)
                    {
                        InTextSegment = currentOpenToken.Value.Item2.CloseValid || info.OpenValid
                    };
                    currentOpenToken = null;
                }
            }
        }
        
        public Tag GetBaseTag()
        {
            return tag;
        }

        public bool Contain(TokenSegment other)
        {
            if (other is null) throw new ArgumentNullException();

            var firstMin = Math.Min(openTokenLocation, closeTokenLocation);
            var firstMax = Math.Max(openTokenLocation, closeTokenLocation);
            var secondMin = Math.Min(other.openTokenLocation, other.closeTokenLocation);
            var secondMax = Math.Max(other.openTokenLocation, other.closeTokenLocation);

            return secondMin.Between(firstMin, firstMax) && secondMax.Between(firstMin, firstMax);
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

        public bool IsEmpty()
        {
            return InnerLength == 0;
        }
    }
}