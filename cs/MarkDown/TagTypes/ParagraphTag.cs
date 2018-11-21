using System;

namespace MarkDown.TagTypes
{
    public class ParagraphTag : TagType
    {
        public ParagraphTag() : base("\n", "\n", "p", new Type[]{}) {}
    }
}