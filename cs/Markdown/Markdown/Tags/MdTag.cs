using System;

namespace Markdown
{
    public class MdTag : Tag
    {
        private MdTag(string tag, bool hasCloseTag)
        {
            OpenTag = tag;
            if (hasCloseTag)
                CloseTag = tag;
            else
                CloseTag = String.Empty;
        }
        
        public override string CreateStringWithTag(string str)
        {
            return String.Empty;
        }
    }
}
