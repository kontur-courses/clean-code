using System;

namespace Markdown
{
    public class MdTag : Tag
    {
        public MdTag(string tag, bool hasCloseTag): 
            this(tag, hasCloseTag, -1, -1) { }

        public MdTag(string tag, bool hasCloseTag, int openTagIndex, int closeTagIndex)
        {
            OpenTag = tag;
            if (hasCloseTag)
                CloseTag = tag;
            else
                CloseTag = String.Empty;
            OpenTagIndex = openTagIndex;
            CloseTagIndex = closeTagIndex;
        }

        public bool IsSimpleTag()
        {
            return OpenTag.Length == 1;
        }
        
        public override string CreateStringWithTag(string str)
        {
            return String.Empty;
        }
    }
}
