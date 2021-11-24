using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Extensions;

namespace Markdown
{
    internal class TokenSegment : IEqualityComparer<TokenSegment>
    {
        private Tag tag;

        public bool InTextSegment { get; }
        public int StartPosition { get; }
        public int EndPosition { get; }
        public int Length => EndPosition - StartPosition;
        public int InnerLength => EndPosition - (StartPosition + tag.Start.Length);
        
        internal TokenSegment(){}
        // private TokenSegment(Tag tag, int openTokenLocation, int closeTokenLocation)
        // {
        //     if (tag is null) throw new ArgumentNullException();
        //     if (tag.End is null && openTokenLocation != closeTokenLocation)
        //         throw new AggregateException("single tag open and close in different location");
        //
        //     this.tag = tag;
        //     StartPosition = openTokenLocation;
        //     EndPosition = closeTokenLocation;
        // }

        internal TokenSegment(TokenInfo first, TokenInfo second = null)
        {
            if (first is null) throw new ArgumentNullException();

            tag = second is null 
                ? Tag.GetOrAddSingleTag(first.Token) 
                : Tag.GetOrAddSymmetricTag(first.Token);
            
            StartPosition = first.Position;
            EndPosition = second?.Position ?? first.Position;
            InTextSegment = first.WordPartPlaced || (second?.WordPartPlaced ?? false);
        }

        // public static IEnumerable<TokenSegment> GetTokensSegments(IEnumerable<TokenInfo> tokensByLocation)
        // {
        //     if (tokensByLocation is null) throw new ArgumentNullException();
        //     
        //     (int, TokenInfo)? currentOpenToken = null;
        //     foreach (var info in tokensByLocation.OrderBy(x => x.Position))
        //     {
        //         var (index, token, close, open, _, _) = info;
        //         var tag = Tag.GetTagByChars(token);
        //         if (tag.End is null)
        //         {
        //             yield return new TokenSegment(tag, index, index);
        //         }
        //         else if (currentOpenToken is null && open)
        //             currentOpenToken = (index, info);
        //         else if (currentOpenToken is not null && close)
        //         {
        //             yield return new TokenSegment(Tag.GetTagByChars(currentOpenToken.Value.Item2.Token), currentOpenToken.Value.Item1, index)
        //             {
        //                 InTextSegment = currentOpenToken.Value.Item2.CloseValid || info.OpenValid
        //             };
        //             currentOpenToken = null;
        //         }
        //     }
        // }
        
        public Tag GetBaseTag()
        {
            return tag;
        }

        public bool Contain(TokenSegment other)
        {
            if (other is null) throw new ArgumentNullException();

            var firstMin = Math.Min(StartPosition, EndPosition);
            var firstMax = Math.Max(StartPosition, EndPosition);
            var secondMin = Math.Min(other.StartPosition, other.EndPosition);
            var secondMax = Math.Max(other.StartPosition, other.EndPosition);

            return secondMin.IsBetween(firstMin, firstMax) && secondMax.IsBetween(firstMin, firstMax);
        }
        
        public bool IsIntersectWith(TokenSegment other)
        {
            if (other is null) throw new ArgumentNullException();

            var firstMin = Math.Min(StartPosition, EndPosition);
            var firstMax = Math.Max(StartPosition, EndPosition);
            var secondMin = Math.Min(other.StartPosition, other.EndPosition);
            var secondMax = Math.Max(other.StartPosition, other.EndPosition);

            return firstMin.IsBetween(secondMin, secondMax) && !firstMax.IsBetween(secondMin, secondMax)
                   || !firstMin.IsBetween(secondMin, secondMax) && firstMax.IsBetween(secondMin, secondMax);
        }

        public bool IsEmpty()
        {
            return InnerLength == 0;
        }

        public bool Equals(TokenSegment x, TokenSegment y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.StartPosition == y.StartPosition && x.EndPosition == y.EndPosition && Equals(x.tag, y.tag) && x.InTextSegment == y.InTextSegment;
        }

        public int GetHashCode(TokenSegment obj)
        {
            return HashCode.Combine(obj.StartPosition, obj.EndPosition, obj.tag, obj.InTextSegment);
        }
    }
}