using System;

namespace Markdown
{
    public class MdTag : Tag
    {
        public MdTag(string tag, bool hasCloseTag)
        {
            OpenTag = tag;
            if (hasCloseTag)
                CloseTag = tag;
            else
                CloseTag = String.Empty;
            OpenTagIndex = -1;
            CloseTagIndex = -1;
        }
        
        public override string CreateStringWithTag(string str)
        {
            return String.Empty;
        }
    }
}
