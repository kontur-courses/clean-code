using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.HtmlTag;

namespace Markdown.Markdown
{
    internal class Tag
    {
        public static readonly Dictionary<string, HtmlTag.HtmlTag> Tags =
            new Dictionary<string, HtmlTag.HtmlTag>
            {
                ["_"] = new("Italic"),
                ["__"] = new ("Strong"),
                ["# "] = new ("h1")
            };

        public HtmlTag.HtmlTag HtmlTag;
        public int Index;

        public Tag(string mdTag, int index)
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
