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
            var sortedTags = GetTagsFromPairs(tagsPairs);
            var newString = new StringBuilder();
            var currentIndex = 0;
            foreach (var tag in sortedTags)
            {
                newString.Append(sourceStrings, currentIndex, tag.PositionInText - currentIndex);
                var newTagSpecification = newSpecifications.FirstOrDefault(t => t.TagType == tag.TagType);
                if (newTagSpecification == null)
                {
                    throw new ArgumentException($"New specifications does not support tag type {tag.TagType}");
                }
                var newTag = tag.PositionType == PositionType.OpeningTag ? newTagSpecification.StartTag : newTagSpecification.EndTag;
                newString.Append(newTag);
                currentIndex = tag.PositionInText + tag.Value.Length;
            }
            if (sourceStrings.Length - currentIndex > 0)
                newString.Append(sourceStrings, currentIndex, sourceStrings.Length - currentIndex);
            return newString.ToString();
        }

        private static IEnumerable<Tag> GetTagsFromPairs(IEnumerable<TagsPair> tagsPairs)
        {
            var sortedTags = new SortedList<int, Tag>();
            foreach (var pair in tagsPairs)
            {
                sortedTags.Add(pair.StartPosition, pair.StartTag);
                sortedTags.Add(pair.EndPosition, pair.EndTag);
            }

            return sortedTags.Values;
        }
    }
}
