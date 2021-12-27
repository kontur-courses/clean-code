using System.Collections.Generic;
using System.Linq;
using MarkdownTask.Tags;

namespace MarkdownTask
{
    public class TagsInspector
    {
        private readonly HashSet<(TagType, TagType)> shouldNotContain;
        private readonly HashSet<(TagType, TagType)> shouldNotIntersect;

        public TagsInspector()
        {
            shouldNotContain = new HashSet<(TagType, TagType)>();
            shouldNotIntersect = new HashSet<(TagType, TagType)>();
        }

        public List<Tag> InspectTags(List<Tag> tags)
        {
            tags = RemoveIntersection(tags);
            tags = RemoveContaining(tags);

            return tags;
        }

        public TagsInspector ExcludeIntersection(TagType first, TagType second)
        {
            shouldNotIntersect.Add((first, second));
            return this;
        }

        public TagsInspector ExcludeContaining(TagType outerType, TagType innerType)
        {
            shouldNotContain.Add((outerType, innerType));
            return this;
        }

        private List<Tag> RemoveIntersection(List<Tag> tags)
        {
            foreach (var (firstType, secondType) in shouldNotIntersect)
            {
                if (ShouldSkipTypes(tags, (firstType, secondType)))
                    continue;

                var intersectedTags = new List<Tag>();

                foreach (var firstTag in GetTagsOfType(tags, firstType))
                foreach (var secondTag in GetTagsOfType(tags, secondType))
                    if (firstTag.IntersectsWith(secondTag))
                    {
                        intersectedTags.Add(firstTag);
                        intersectedTags.Add(secondTag);
                    }

                tags = tags.Where(tag => !intersectedTags.Contains(tag)).ToList();
            }

            return tags;
        }

        private List<Tag> RemoveContaining(List<Tag> tags)
        {
            foreach (var (outer, inner) in shouldNotContain)
            {
                if (ShouldSkipTypes(tags, (outer, inner)))
                    continue;

                var containedTags = new List<Tag>();

                foreach (var outerTag in GetTagsOfType(tags, outer))
                foreach (var innerTag in GetTagsOfType(tags, inner))
                    if (outerTag.Contains(innerTag))
                        containedTags.Add(innerTag);

                tags = tags.Where(tag => !containedTags.Contains(tag)).ToList();
            }

            return tags;
        }

        private bool ShouldSkipTypes(
            IEnumerable<Tag> tags,
            (TagType first, TagType second) types)
        {
            var isExistTagsOfFirstType = tags.Any(tag => tag.TagTagStyleInfo.Type == types.first);
            var isExistTagsOfSecondType = tags.Any(tag => tag.TagTagStyleInfo.Type == types.second);

            return !isExistTagsOfFirstType || !isExistTagsOfSecondType;
        }

        private IEnumerable<Tag> GetTagsOfType(IEnumerable<Tag> tags, TagType type)
        {
            return tags.Where(tag => tag.TagTagStyleInfo.Type == type);
        }
    }
}