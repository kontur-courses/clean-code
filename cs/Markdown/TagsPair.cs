using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class TagsPair
    {
        public readonly TagType PairType;
        public int StartPosition;
        public int EndPosition;
        public readonly Tag StartTag;
        public readonly Tag EndTag;

        public TagsPair(TagType type, Tag startTag, Tag endTag)
        {
            PairType = type;
            StartTag = startTag;
            EndTag = endTag;
            EndPosition = EndTag.PositionInText;
            StartPosition = startTag.PositionInText;
        }
    }
}
