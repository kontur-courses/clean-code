using System.Collections.Generic;

namespace MarkDown.TagParsers
{
    public class EmParser : TagParser
    {
        public override Stack<TagType> EscapingTokens => new Stack<TagType>();
        public override TagType Type => TagType.Em;
        public override (string md, string html) OpeningTags => ("_", @"<em>");
        public override (string md, string html) ClosingTags => ("_", @"</em>");
    }
}