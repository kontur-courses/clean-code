using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Element
{
    public class HeaderHtmlElement : IElement
    {
        public string Indicator { get; set; }
        public string OpenTag { get; set; }
        public string ClosingTag { get; set; }

        public string HtmlTag { get; set; }

        public HeaderHtmlElement(string indicator, string htmlTag)
        {
            HtmlTag = htmlTag;
            Indicator = indicator;
            OpenTag = $"<{HtmlTag}>";
            ClosingTag = $"</{HtmlTag}>";
        }

        public HeaderHtmlElement(string htmlTag, int level, string indicator)
        {
            HtmlTag = htmlTag;
            Indicator = indicator;
            OpenTag = $"<{HtmlTag}{level}>";
            ClosingTag = $"</{HtmlTag}{level}>";
        }
    }
}
