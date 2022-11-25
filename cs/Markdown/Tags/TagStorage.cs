using System.Collections.Generic;

namespace Markdown.Tags
{
    public abstract class TagStorage
    {
        public List<ITag> Tags { get; protected set; }

        public Dictionary<TagType, HashSet<TagType>> ForbiddenTagNestings { get; protected set; }

        public string EscapeCharacter => "\\" ;

        public string GetSubTag(TagType tagType, SubTagOrder order)
        {
            var tag = Tags.Find(t => t.Type == tagType);

            return order == SubTagOrder.Opening ? tag.OpeningSubTag : tag.ClosingSubTag;
        }

        public bool IsForbiddenTagNesting(TagType outerTagType, TagType innerTagType)
        {
            if (ForbiddenTagNestings.TryGetValue(outerTagType, out HashSet<TagType> forbiddenTagsToNesting))
            {
                if (forbiddenTagsToNesting.Contains(innerTagType))
                    return true;
            }

            return false;
        }
    }
}
