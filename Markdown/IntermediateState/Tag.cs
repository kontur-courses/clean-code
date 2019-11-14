using System.Collections.Generic;

namespace Markdown.IntermediateState
{
    class Tag
    {
        public string OpenTag { get; }
        public string CloseTag { get; }
        public TagType TypeTag { get; }

        private HashSet<TagType> ignoredInnerTags;

        public Tag(TagType tagType, string openTag, string closeTag)
        {
            OpenTag = openTag;
            CloseTag = closeTag;
            TypeTag = tagType;
        }

        public void AddIgnoredTag(TagType tagType)
        {
            IgnoredInnerTags.Add(tagType);
        }

        public bool CanTagUsedInCurrent(TagType tag)
        {
            return !IgnoredInnerTags.Contains(tag);
        }
    }
}
