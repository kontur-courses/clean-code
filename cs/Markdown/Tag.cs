using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    class Tag
    {
        public readonly string markdownTag;
        public readonly string htmlStartTag;
        public readonly string htmlEndTag;
        public Tag(string markdownTag, string htmlStartTag, string htmlEndTag)
        {
            this.markdownTag = markdownTag;
            this.htmlStartTag = htmlStartTag;
            this.htmlEndTag = htmlEndTag;
        }
    }
}
