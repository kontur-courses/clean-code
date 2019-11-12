using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class TagsPair
    {
        public readonly TagType PairType;
        public readonly int StartPosition;
        public readonly int EndPosition;

        public TagsPair(TagType type, int start, int end)
        {
            PairType = type;
            StartPosition = start;
            EndPosition = end;
        }
    }
}
