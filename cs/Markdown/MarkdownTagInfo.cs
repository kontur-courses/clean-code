using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class MarkdownTagInfo
    {
        public Tag? Tag { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public bool WhitespacesBefore { get; set; }
        public bool InNumber { get; set; }
        public bool InWord { get; set; }
        public bool IsOpening { get; set; }
        public bool IsClosing { get; set; }

        public MarkdownTagInfo(Tag tag = null)
        {
            Tag = tag;
        }
    }
}
