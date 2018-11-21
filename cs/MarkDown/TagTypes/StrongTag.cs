using System;

namespace MarkDown.TagTypes
{
    public class StrongTag : TagType
    {
        public StrongTag() : base("__", "__", "strong", new Type[]{ typeof(StrongTag), typeof(ATag)}) { }
    }
}