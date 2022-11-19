using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    internal class Tag
    {
        public static readonly Dictionary<string, HtmlTag> Tags =
            new Dictionary<string, HtmlTag>
            {
                ["_"] = new HtmlTag("Italic"),
                ["__"]= new HtmlTag("Strong"),
                ["# "]= new HtmlTag("title")
            };
        public HtmlTag HtmlTag;
        public int Index;
        public Tag(string mdTag,int index)
        {
            HtmlTag = Tags[mdTag];
            Index = index;
        }

        public static bool IsMarkdownTag(string mdTag)
        {
            return Tags.ContainsKey(mdTag);
        }
    }
}
