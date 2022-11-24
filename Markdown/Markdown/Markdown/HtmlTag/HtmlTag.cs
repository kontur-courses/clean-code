using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.HtmlTag
{
    public class HtmlTag
    {
        public readonly string StartTag;
        public readonly string EndTag;

        public HtmlTag(string tag)
        {
            if (string.IsNullOrEmpty(tag))
                throw new ArgumentNullException("Tag must be with arguments");
            StartTag = $@"\<{tag}>";
            EndTag = $@"\</{tag}>";
        }
        public HtmlTag(string startTag, string endTag)
        {
            if (string.IsNullOrEmpty(startTag) || string.IsNullOrEmpty(endTag))
                throw new ArgumentNullException("Tag must be with arguments");
            StartTag = startTag;
            EndTag = endTag;
        }

        public static string CreateHtmlString(string stringForHtml, HtmlTag htmlTag)
        {
            return htmlTag.StartTag + stringForHtml + htmlTag.EndTag;
        }
    }
}
