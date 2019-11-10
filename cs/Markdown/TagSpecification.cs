using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class TagSpecification
    {
        public readonly string StartTag;
        public readonly string EndTag;
        public readonly TagType TagType;
        public readonly List<string> IgnoreTags;

        public TagSpecification(string startTag, string endTag, TagType tagType, List<string> ignoreTags = null)
        {
            StartTag = startTag;
            EndTag = endTag;
            TagType = tagType;
            IgnoreTags = ignoreTags ?? new List<string>();
        }
    }
}
