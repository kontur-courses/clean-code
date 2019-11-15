using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public static class TagsParser
    {

        private const char EscapeSymbol = '\\';

        public static List<TagsPair> GetTagsPair(StringBuilder sourceText, List<TagSpecification> currentSpecifications)
        {
            var tagsSequences = GetTagsSequences(sourceText, currentSpecifications);
            var tagsPair = GetTagsPair(tagsSequences, currentSpecifications);
            return tagsPair;
        }

        private static Queue<Tag> GetTagsSequences(StringBuilder sourceString, List<TagSpecification> currentSpecifications)
        {
            var result = new Queue<Tag>();
            for (var currentPosition = 0; currentPosition < sourceString.Length; currentPosition++)
            {
                if (EscapeSymbol == sourceString[currentPosition])
                {
                    sourceString.Remove(currentPosition, 1);
                    continue;
                }
                var possibleStartTag = GetAllPossibleTag(currentSpecifications, sourceString, currentPosition, 
                    tag => tag.StartTag.StartsWith(sourceString[currentPosition]), CanBeOpenTag).FirstOrDefault();
                var possibleEndTag = GetAllPossibleTag(currentSpecifications, sourceString, currentPosition, 
                    tag => tag.EndTag.StartsWith(sourceString[currentPosition]), CanBeEndTag).FirstOrDefault();
                if (possibleEndTag == null && possibleStartTag == null)
                {
                    continue;
                }
                var tag = GetMostPriorityTagInCurrentPosition(currentSpecifications, possibleStartTag, possibleEndTag, currentPosition);
                result.Enqueue(tag);
                if (tag.positionType == PositionType.OpeningTag || tag.positionType == PositionType.any)
                    currentPosition += currentSpecifications.First(tag1 => tag1.TagType == tag.TagType).StartTag.Length;
                else
                    currentPosition += currentSpecifications.First(tag1 => tag1.TagType == tag.TagType).EndTag.Length;

            }
            return result;
        }

        private static Tag GetMostPriorityTagInCurrentPosition(List<TagSpecification> currentSpecifications, TagSpecification possibleStartTag, TagSpecification possibleEndTag, int currentPosition)
        {
            var priorityEnd = possibleEndTag == null ? int.MaxValue : currentSpecifications.FindIndex(0,
                specification => specification.TagType == possibleEndTag.TagType);
            var priorityStart = possibleStartTag == null ? int.MaxValue : currentSpecifications.FindIndex(0,
                specification => specification.TagType == possibleStartTag.TagType);
            var positionType = priorityStart == priorityEnd ? PositionType.any : priorityStart < priorityEnd ? PositionType.OpeningTag : PositionType.ClosingTag;
            var tagType = priorityStart <= priorityEnd ? possibleStartTag.TagType : possibleEndTag.TagType;
            return new Tag(tagType, currentPosition, positionType);
        }

        private static List<TagSpecification> GetAllPossibleTag(List<TagSpecification> currentSpecifications,
            StringBuilder sourceString, int currentPosition, Func<TagSpecification, bool> inNeedPositionType, Func<string, int, StringBuilder, bool> canBeInNeedPosition)
        {
            return currentSpecifications.FindAll(tag => inNeedPositionType(tag))
                .Where(possibleTag => IsValidTag(sourceString, possibleTag.StartTag, currentPosition, canBeInNeedPosition))
                .ToList();
        }

        private static bool IsValidTag(StringBuilder sourceString, string possibleTag, int currentPosition, Func<string, int, StringBuilder, bool> validatePosition)
        {
            var lengthPossibleTag = possibleTag.Length;
            if (lengthPossibleTag + currentPosition > sourceString.Length)
            {
                return false;
            }
            return sourceString.ToString(currentPosition, lengthPossibleTag).Equals(possibleTag)
                   && validatePosition(possibleTag, currentPosition, sourceString) && !HaveDigitAround(possibleTag, currentPosition, sourceString);
        }

        private static bool HaveDigitAround(string symbol, int position, StringBuilder text)
        {
            if (position == text.Length - symbol.Length)
                return char.IsDigit(text.ToString(text.Length - symbol.Length - 1, 1)[0]);
            if (position == 0)
                return char.IsDigit(text.ToString(symbol.Length, 1)[0]);
            return char.IsDigit(text.ToString(position - 1, 1)[0]) || char.IsDigit(text.ToString(position + symbol.Length, 1)[0]);
        }

        private static List<TagsPair> GetTagsPair(Queue<Tag> characterSequences, List<TagSpecification> currentSpecifications)
        {
            var previousPairs = new List<TagsPair>();
            var openingTags = new List<Tag>();
            while (characterSequences.Count > 0)
            {
                var currentTag = characterSequences.Dequeue();
                var startTag = openingTags.FindLast(t => t.TagType == currentTag.TagType);
                if (currentTag.positionType == PositionType.OpeningTag)
                {
                    openingTags.Add(currentTag);
                    continue;
                }
                if (startTag == null)
                {
                    if (currentTag.positionType == PositionType.any)
                    {
                        openingTags.Add(currentTag);
                    }
                    continue;
                }
                var currentPair = new TagsPair(currentTag.TagType, startTag.PositionInText, currentTag.PositionInText);
                openingTags.Remove(startTag);
                foreach (var invalidOpeningTags in GetAllInvalidOpeningTag(currentPair, openingTags, currentSpecifications))
                {
                    openingTags.Remove(invalidOpeningTags);
                }
                foreach (var invalidTagsPair in GetAllInvalidTagsPair(currentPair, previousPairs, currentSpecifications))
                {
                    previousPairs.Remove(invalidTagsPair);
                    if (invalidTagsPair.StartPosition > currentPair.StartPosition) continue;
                    var tagForAdd = new Tag(invalidTagsPair.PairType, invalidTagsPair.StartPosition, PositionType.OpeningTag);
                    var indexForInsert = openingTags.FindIndex(0, t => t.PositionInText > invalidTagsPair.StartPosition);
                    openingTags.Insert(indexForInsert == -1 ? 0 : indexForInsert, tagForAdd);
                }
                previousPairs.Add(currentPair);
            }
            return previousPairs;
        }

        private static IEnumerable<Tag> GetAllInvalidOpeningTag(TagsPair currentPair, IReadOnlyCollection<Tag> previousOpeningTags, IReadOnlyCollection<TagSpecification> currentSpecifications)
        {
            var specificationForCurrentPairType = currentSpecifications.First(t => t.TagType == currentPair.PairType);
            return previousOpeningTags.Where(tag => tag.PositionInText > currentPair.StartPosition).Where(tag => specificationForCurrentPairType.IgnoreTags.Contains(tag.TagType)).ToList();
        }

        private static IEnumerable<TagsPair> GetAllInvalidTagsPair(TagsPair currentPair, IReadOnlyCollection<TagsPair> previousPairs,
            IReadOnlyCollection<TagSpecification> currentSpecifications)
        {
            var specificationForCurrentPairType = currentSpecifications.First(t => t.TagType == currentPair.PairType);
            return previousPairs.Where(pair => pair.EndPosition > currentPair.StartPosition).Where(tagPair => specificationForCurrentPairType.IgnoreTags.Contains(tagPair.PairType)).ToList();
        }

        private static bool CanBeOpenTag(string symbol, int position, StringBuilder text)
        {
            if (position == text.Length - symbol.Length)
            {
                return false;
            }
            var str = text.ToString(position + symbol.Length, 1);
            return !string.IsNullOrWhiteSpace(str);
        }

        private static bool CanBeEndTag(string symbol, int position, StringBuilder text)
        {
            if (position == 0)
            {
                return false;
            }
            var str = text.ToString(position - 1, 1);
            return !string.IsNullOrWhiteSpace(str);
        }
    }
}
