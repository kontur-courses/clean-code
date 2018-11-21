using System;

namespace MarkDown.TagTypes
{
    public class EmTag : TagType
    {
        public EmTag() : base("_", "_", "em", new Type[] {typeof(StrongTag), typeof(EmTag), typeof(ATag)}) { }
    }
}