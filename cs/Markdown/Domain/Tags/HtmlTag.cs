using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Domain.Tags
{
    public class HtmlTag
    {
        public Tag Tag { get; }
        public int Index { get; }
        public bool IsClosing { get; }
        public string Markup { get; }

        public HtmlTag(Tag tag, int index, bool isClosing, string htmlMarkup)
        {
            Tag = tag;
            Index = index;
            IsClosing = isClosing;
            Markup = htmlMarkup;
        }
    }
}
