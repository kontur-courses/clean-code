using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Linq;
using System.Runtime.InteropServices;

namespace Markdown
{
    public class Md
    {
        public static string Render(string sourceText)
        {
            var currentText = new StringBuilder(sourceText);
            var tagsPair = TagsParser.GetTagsPair(currentText, MdTags.GetAllTags());
            var result = ConvertToNewSpecifications(currentText, HTMLTags.GetAllSpecifications(), tagsPair);
            return result;
        }

        private static string ConvertToNewSpecifications(StringBuilder sourceStrings, List<TagSpecification> newSpecifications, List<TagsPair> tagsPair)
        {
            var currentSpecifications = MdTags.GetAllTags();
            var sortedTags = new SortedList<int, Tag>();
            foreach (var pair in tagsPair)
            {
                sortedTags.Add(-pair.StartPosition, new Tag(pair.PairType, pair.StartPosition, PositionType.OpeningTag));
                sortedTags.Add(-pair.EndPosition, new Tag(pair.PairType, pair.EndPosition, PositionType.ClosingTag));
            }
            foreach (var tag in sortedTags.Values)
            {
                var tagForReplace = currentSpecifications.First(t => t.TagType == tag.TagType);
                var oldTag = tag.positionType == PositionType.OpeningTag ? tagForReplace.StartTag : tagForReplace.EndTag;
                var newTags = newSpecifications.FirstOrDefault(t => t.TagType == tag.TagType);
                if (newTags == null)
                {
                    throw  new ArgumentException($"New specifications does not support tag type {tagForReplace.TagType}");
                }
                var newTag = tag.positionType == PositionType.OpeningTag ? newTags.StartTag : newTags.EndTag;
                sourceStrings.Replace(oldTag, newTag, tag.PositionInText, oldTag.Length);
            }
            return sourceStrings.ToString();
        }
    }
}
