using System;

namespace Markdown
{
    public class MdElement
    {
        public bool IsEnclosed;

        public char MdTag;

        public string HtmlTag;

        public string HtmlTagClose;

        public MdElement(char mdTag, string htmlTag, bool isEnclosed)
        {
            IsEnclosed = isEnclosed;
            MdTag = mdTag;
            HtmlTag = htmlTag ?? throw new ArgumentNullException(nameof(htmlTag));
            HtmlTagClose = htmlTag.Substring(0, htmlTag.Length - 1) + "/" + ">";
        }
    }
}
