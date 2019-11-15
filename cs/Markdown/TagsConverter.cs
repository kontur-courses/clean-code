using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public static class TagsConverter
    {
        public static string ConvertToNewSpecifications(StringBuilder sourceStrings, IReadOnlyCollection<TagSpecification> currentSpecifications, IReadOnlyCollection<TagSpecification> newSpecifications, IEnumerable<TagsPair> tagsPairs)
        {
            var sortedTags = GetTagsFromPairs(tagsPairs).Reverse();
            foreach (var tag in sortedTags)
            {
                var tagForReplace = currentSpecifications.First(t => t.TagType == tag.TagType);
                var oldTag = tag.positionType == PositionType.OpeningTag ? tagForReplace.StartTag : tagForReplace.EndTag;
                var newTagSpecification = newSpecifications.FirstOrDefault(t => t.TagType == tag.TagType);
                if (newTagSpecification == null)
                {
                    throw new ArgumentException($"New specifications does not support tag type {tagForReplace.TagType}");
                }
                var newTag = tag.positionType == PositionType.OpeningTag ? newTagSpecification.StartTag : newTagSpecification.EndTag;
                sourceStrings.Replace(oldTag, newTag, tag.PositionInText, oldTag.Length);
            }
            return sourceStrings.ToString();
        }

        private static IEnumerable<Tag> GetTagsFromPairs(IEnumerable<TagsPair> tagsPairs)
        {
            var sortedTags = new SortedList<int, Tag>();
            foreach (var pair in tagsPairs)
            {
                sortedTags.Add(pair.StartPosition, new Tag(pair.PairType, pair.StartPosition, PositionType.OpeningTag));
                sortedTags.Add(pair.EndPosition, new Tag(pair.PairType, pair.EndPosition, PositionType.ClosingTag));
            }

            return sortedTags.Values;
        }
    }
}
