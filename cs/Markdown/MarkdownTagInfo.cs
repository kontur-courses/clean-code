using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.TagClasses;

namespace Markdown
{
    public class MarkdownTagInfo
    {
        public Tag? Tag { get; private set; }
        public int StartIndex { get; private set; }
        public int EndIndex { get; private set; }

        public MarkdownTagInfo(Tag? tag, int startIndex, int endIndex)
        {
            Tag = tag;
            StartIndex = startIndex;
            EndIndex = endIndex;
        }

        public override string ToString()
        {
            return $"{Tag} {StartIndex}";
        }
    }
}
