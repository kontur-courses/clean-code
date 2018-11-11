using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class Tag
    {
        public string MarkdownStart { get; }
        public string MarkdownEnd { get; }
        public string HtmlStart { get; }
        public string HtmlEnd { get; }

        public Tag(string markdownStart, string markdownEnd, string htmlStart, string htmlEnd)
        {
            MarkdownStart = markdownStart;
            MarkdownEnd = markdownEnd;
            HtmlStart = htmlStart;
            HtmlEnd = htmlEnd;
        }

        public override int GetHashCode()
        {
            return (MarkdownStart.GetHashCode() ^ MarkdownEnd.GetHashCode()) *
                   (HtmlStart.GetHashCode() ^ HtmlEnd.GetHashCode());
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() == this.GetType())
                return false;

            var otherTag = (Tag) obj;
            return MarkdownStart == otherTag.MarkdownStart && MarkdownEnd == otherTag.MarkdownEnd &&
                   HtmlStart == otherTag.HtmlStart && HtmlEnd == otherTag.HtmlEnd;
        }

    }
}
