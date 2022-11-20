using System;

namespace Markdown
{
    public abstract class Tag
    {
        public string OpenTag { get; protected set; }
        public string CloseTag { get; protected set; }
        public int OpenTagIndex { get; protected set; }
        public int CloseTagIndex { get; protected set; }
        
        public bool HasCloseTag => CloseTag != String.Empty;
        public abstract string CreateStringWithTag(string str);
    }
}
