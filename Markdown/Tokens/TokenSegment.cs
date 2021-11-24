using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    internal class TokenSegment
    {
        private readonly Tag tag;

        public int StartPosition { get; }
        public int EndPosition { get; }
        private int InnerLength => EndPosition - (StartPosition + tag.Start.Length);
        
        internal TokenSegment(){}

        internal TokenSegment(TokenInfo first, TokenInfo second = null)
        {
            if (first is null) throw new ArgumentNullException();

            tag = second is null 
                ? Tag.GetOrAddSingleTag(first.Token) 
                : Tag.GetOrAddSymmetricTag(first.Token);
            
            StartPosition = first.Position;
            EndPosition = second?.Position ?? first.Position;
        }
        
        public Tag GetBaseTag()
        {
            return tag;
        }

        public bool IsEmpty()
        {
            return InnerLength == 0;
        }
    }
}