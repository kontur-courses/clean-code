using System.Collections.Generic;
using Markdown.Core.Infrastructure;

namespace Markdown.Core.Normalizer
{
    public class IgnoreInsideRule
    {
        public readonly List<TagInfo> OuterTags;
        public readonly TagInfo IgnoredInsideTag;

        public IgnoreInsideRule(List<TagInfo> outerTags, TagInfo ignoredInsideTag)
        {
            OuterTags = outerTags;
            IgnoredInsideTag = ignoredInsideTag;
        }
    }
}