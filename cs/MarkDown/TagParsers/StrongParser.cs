using System.Collections.Generic;

namespace MarkDown.TagParsers
{
    public class StrongParser : TagParser
    {
        public override Stack<TagType> EscapingTokens => new Stack<TagType>(new[] {TagType.Em});
        public override TagType Type => TagType.Strong;
        public override (string md, string html) OpeningTags => ("__", @"<strong>");
        public override (string md, string html) ClosingTags => ("__", @"</strong>");
    }
}