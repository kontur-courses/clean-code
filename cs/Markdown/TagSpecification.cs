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
        public readonly List<TagType> IgnoreTags;
        public readonly bool EndWithLine;

        public TagSpecification(string startTag, string endTag, TagType tagType, List<TagType> ignoreTags = null, bool endWithEndLine = false)
        {
            StartTag = startTag;
            EndTag = endTag;
            TagType = tagType;
            IgnoreTags = ignoreTags ?? new List<TagType>();
            EndWithLine = endWithEndLine;
        }
    }
}
