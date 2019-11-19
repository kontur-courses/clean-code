using System;
using System.Collections.Generic;
using System.Text;

namespace MarkDown.TokenParsers
{
    public class Tag
    {
        public readonly int Length;
        public readonly int StartIndex;
        public TokenType Type;
        public int indexNextToTag => StartIndex + Length;
        public int EndedIndex => StartIndex + Length - 1;

        public Tag(int startIndex, int length, TokenType type)
        {
            if (length < 0) throw new ArgumentException("Length can't be less than zero");
            if (startIndex < 0) throw new ArgumentException("StartIndex can't be less than zero");
            StartIndex = startIndex;
            Length = length;
            Type = type;
        }
    }
}
