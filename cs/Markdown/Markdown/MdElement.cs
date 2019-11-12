using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class MdElement
    {
        public MdElement(char mdTag, string htmlTag, bool isEnclosed)
        {
            if (htmlTag == null)
                throw new ArgumentNullException(nameof(htmlTag));
            IsEnclosed = isEnclosed;
            MdTag = mdTag;
            HtmlTag = htmlTag;
            HtmlTagClose = htmlTag.Substring(0, htmlTag.Length - 1) + "/" + ">";
        }

        public bool IsEnclosed;

        public char MdTag;

        public string HtmlTag;

        public string HtmlTagClose;
    }
}
