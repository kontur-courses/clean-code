using System.Collections.Generic;

namespace MarkDown.TagTypes
{
    public class EmTag : TagType
    {
        public EmTag() : base("_", "em", new List<TagType>()) { }
    }
}