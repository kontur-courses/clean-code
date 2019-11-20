using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class TagSpecification
    {
        public readonly string StartToken;
        public readonly string EndToken;
        public readonly TagType TagType;
        public readonly List<TagType> IgnoreTags;
        public readonly bool EndWithLine;

        public TagSpecification(string startToken, string endToken, TagType tagType, List<TagType> ignoreTags = null, bool endWithEndLine = false)
        {
            StartToken = startToken;
            EndToken = endToken;
            TagType = tagType;
            IgnoreTags = ignoreTags ?? new List<TagType>();
            EndWithLine = endWithEndLine;
        }
    }
}
