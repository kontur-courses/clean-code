using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class Tag
    {
        public readonly TagType TagType;
        public readonly PositionType positionType;
        public readonly int PositionInText;

        public Tag(TagType tagType, int positionInText, PositionType positionType)
        {
            this.TagType = tagType;
            this.PositionInText = positionInText;
            this.positionType = positionType;
        }
    }
}
