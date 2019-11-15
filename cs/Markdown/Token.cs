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

        public Token(int position, int length, TagType tagType)
        {
            Position = position;
            TagType = tagType;
            Length = length;
        }
        
    }
}