using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    internal class TokenSegment
    {
        private Tag tag;

        public bool InTextSegment { get; }
        public int StartPosition { get; }
        public int EndPosition { get; }
        public int InnerLength => EndPosition - (StartPosition + tag.Start.Length);
        
        internal TokenSegment(){}

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
    }
}