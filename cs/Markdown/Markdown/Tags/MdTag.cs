using System;

namespace Markdown
{
    public class MdTag : ITag
    {
        public string OpenTag { get; }
        public string CloseTag { get; }
        public int OpenTagIndex { get; }
        public int CloseTagIndex { get; }
        
        public bool HasCloseTag => CloseTag != String.Empty;
        
        public MdTag(string tag, int openTagIndex): 
            this(tag, false, openTagIndex, -1) { }

        public MdTag(string tag, int openTagIndex, int closeTagIndex): 
            this(tag, true, openTagIndex, closeTagIndex) { }

        private MdTag(string tag, bool hasCloseTag, int openTagIndex, int closeTagIndex)
        {
            OpenTag = tag;
            OpenTagIndex = openTagIndex;
            CloseTagIndex = closeTagIndex;
            if (hasCloseTag)
                CloseTag = tag;
            else
                CloseTag = String.Empty;
        }
        
        public string CreateStringWithTag(string str)
        {
            return String.Empty;
        }
    }
}
