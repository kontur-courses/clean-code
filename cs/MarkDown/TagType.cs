using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkDown
{
    public abstract class TagType
    {
        public string SpecialSymbol { get;}
        public string HtmlTag { get; }

        protected TagType(string specialSymbol, string htmlTag)
        {
            SpecialSymbol = specialSymbol;
            this.HtmlTag = htmlTag;
        }

        public string ToHtml(string text) => $"<{HtmlTag}>{text}</{HtmlTag}>";
    }

    public class EmTag : TagType
    {
        public EmTag() : base("_", "em") { }
    }

    public class StrongTag : TagType
    {
        public StrongTag() : base("__", "strong") { }
    }

    public class ParagraphTag : TagType
    {
        public ParagraphTag() : base("\n", "p") {}
    }
}
