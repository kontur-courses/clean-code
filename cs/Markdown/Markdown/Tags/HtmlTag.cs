using System;

namespace Markdown
{
    public class HtmlTag : ITag
    {
        public string OpenTag { get; }
        public string CloseTag { get; }
        
        public bool HasCloseTag => CloseTag != String.Empty;
        
        public HtmlTag(string tag, bool hasCloseTag)
        {
            OpenTag = $"<{tag}>";
            if (hasCloseTag)
                CloseTag = $"/{tag}";
            else 
                CloseTag = String.Empty;
        }
        
        public string CreateStringWithTag(string str)
        {
            return String.Empty;
        }
    }
}