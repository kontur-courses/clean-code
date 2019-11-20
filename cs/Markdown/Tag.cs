using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class Tag
    {
        public readonly TagType TagType;
        public readonly PositionType PositionType;
        public readonly int PositionInText;
        public readonly string Value;

        public Tag(TagType tagType, int positionInText, PositionType positionType, string value)
        {
            TagType = tagType;
            PositionInText = positionInText;
            PositionType = positionType;
            Value = value;
        }
    }
}
