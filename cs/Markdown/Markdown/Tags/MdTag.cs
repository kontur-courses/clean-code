using System;

namespace Markdown
{
    public class MdTag : Tag
    {
        public MdTag(string tag, bool hasCloseTag)
        {
            OpenTag = tag;
            CloseTag = hasCloseTag ? tag : String.Empty;
        }
    }
}