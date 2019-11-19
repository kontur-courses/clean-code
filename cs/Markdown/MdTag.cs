using System.Collections.Generic;

namespace Markdown
{
    public class MdTag
    {
        public readonly List<MdTag> Child = new List<MdTag>();
        public readonly MdTagDescriptor Descriptor;
        public readonly MdToken LeftBorder;
        public readonly MdToken RightBorder;

        public MdTag(MdToken leftBorder, MdToken rightBorder, MdTagDescriptor descriptor)
        {
            LeftBorder = leftBorder;
            RightBorder = rightBorder;
            Descriptor = descriptor;
        }

        public bool Skipped => LeftBorder.Skipped || RightBorder.Skipped;
    }
}