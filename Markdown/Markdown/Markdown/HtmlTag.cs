using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    internal class HtmlTag
    {
        public readonly string StartTag;
        public readonly string EndTag;
        public HtmlTag(string tag)
        {
            StartTag = $@"<{tag}>/";
            EndTag = $@"/<{tag}>";
            
        }
            
        public static string CreateHtmlString(string stringForHtml, HtmlTag htmlTag)
        { 
            return htmlTag.StartTag+stringForHtml+htmlTag.EndTag;
        }
    }
}
