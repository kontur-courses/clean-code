using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class Token
    {
        public readonly int Position;
        public readonly int Length;
        public readonly TagType TagType;
        public readonly bool HasNestedToken;
        public readonly int NestedTokenCount;

        public Token(int position, int length, TagType tagType, bool hasNestedToken = false, int nestedTokenCount = 0)
        {
            Position = position;
            TagType = tagType;
            Length = length;
            HasNestedToken = hasNestedToken;
            NestedTokenCount = nestedTokenCount;
        }
        
    }
}