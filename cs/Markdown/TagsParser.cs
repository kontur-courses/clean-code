using System;
using System.Collections.Generic;
using System.Dynamic;
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
            var tagsByLine = currentSpecifications.Where(tag =>  tag.EndWithLine).Select(tag => tag.TagType).ToList();
            var tagsPair = GetTagsPair(tagsSequences, currentSpecifications, tagsByLine);
            return tagsPair;
        }

        private static Queue<Tag> GetTagsSequences(StringBuilder sourceString, List<TagSpecification> currentSpecifications)
        {
            var result = new Queue<Tag>();
            var lastWasEndLine = false;
            for (var currentPosition = 0; currentPosition < sourceString.Length; currentPosition++)
            {
                lastWasEndLine = false;
                if (EscapeSymbol == sourceString[currentPosition])
                {
                    sourceString.Remove(currentPosition, 1);
                    continue;
                }
                var possibleStartTag = GetAllPossibleTag(currentSpecifications, sourceString, currentPosition,
                    tagSpecification => tagSpecification.StartTag, CanBeOpenTag).FirstOrDefault();
                var possibleEndTag = GetAllPossibleTag(currentSpecifications, sourceString, currentPosition,
                    tagSpecification => tagSpecification.EndTag, CanBeEndTag).FirstOrDefault();
                if (possibleEndTag == null && possibleStartTag == null)
                {
                    continue;
                }
                var tag = GetMostPriorityTagInCurrentPosition(currentSpecifications, possibleStartTag, possibleEndTag,
                    currentPosition);
                result.Enqueue(tag);
                if (tag.TagType == TagType.EndLine)
                    lastWasEndLine = true;
                currentPosition += tag.Value.Length - 1;
            }
            if (!lastWasEndLine)
                result.Enqueue(new Tag(TagType.EndLine, sourceString.Length, PositionType.ClosingTag, "\r\n"));
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
            var valueTag = priorityStart <= priorityEnd ? possibleStartTag.StartTag : possibleEndTag.EndTag;
            return new Tag(tagType, currentPosition, positionType, valueTag);
        }

        private static IEnumerable<TagSpecification> GetAllPossibleTag(List<TagSpecification> currentSpecifications,
            StringBuilder sourceString, int currentPosition, Func<TagSpecification, string> getNeedPositionTag, Func<string, int, StringBuilder, bool> canBeInNeedPosition)
        {
           return currentSpecifications.Where(tag => getNeedPositionTag(tag).StartsWith(sourceString[currentPosition]))
                .Where(possibleTag => IsValidTag(sourceString, getNeedPositionTag(possibleTag), currentPosition, canBeInNeedPosition));
        }

        private static bool IsValidTag(StringBuilder sourceString, string possibleTag, int currentPosition, Func<string, int, StringBuilder, bool> validatePosition)
        {
            var lengthPossibleTag = possibleTag.Length;
            if (currentPosition + lengthPossibleTag > sourceString.Length)
            {
                return false;
            }
            if (possibleTag == Environment.NewLine)
                return true;
            return  sourceString.ToString(currentPosition, possibleTag.Length).Equals(possibleTag) 
                    && validatePosition(possibleTag, currentPosition, sourceString) 
                    && !HaveDigitAround(possibleTag, currentPosition, sourceString);
        }

        private static bool HaveDigitAround(string symbol, int position, StringBuilder text)
        {
            if (position == text.Length - symbol.Length)
                return char.IsDigit(text[position - 1]);
            if (position == 0)
                return char.IsDigit(text[position + symbol.Length]);
            return char.IsDigit(text[position - 1]) || char.IsDigit(text[position + symbol.Length]);
        }

        private static List<TagsPair> GetTagsPair(Queue<Tag> characterSequences, IReadOnlyCollection<TagSpecification> currentSpecifications, IReadOnlyCollection<TagType> lineSpecifications)
        {
            var previousPairs = new List<TagsPair>();
            var openingTags = new List<Tag>();
            var tagsAfterBlockingTags = new List<TagsPair>();
            while (characterSequences.Count > 0)
            {
                var currentTag = characterSequences.Dequeue();
                if (currentTag.TagType == TagType.EndLine && TryGetLastLineOpenTag(openingTags, lineSpecifications, currentTag, out var newTag))
                    currentTag = newTag;
                var startTag = openingTags.FindLast(t => t.TagType == currentTag.TagType);
                if (CurrentTagCannotCreatePair(startTag, currentTag))
                {
                    if (currentTag.PositionType != PositionType.ClosingTag)
                        openingTags.Add(new Tag(currentTag.TagType, currentTag.PositionInText, PositionType.OpeningTag, currentTag.Value));
                    continue;
                }
                var currentPair = new TagsPair(currentTag.TagType, startTag, currentTag);
                openingTags.Remove(startTag);
                foreach (var invalidOpeningTags in GetAllInvalidOpeningTag(currentPair, openingTags, currentSpecifications))
                    openingTags.Remove(invalidOpeningTags);
                foreach (var invalidTagsPair in GetAllInvalidTagsPair(currentPair, tagsAfterBlockingTags, currentSpecifications))
                {
                    tagsAfterBlockingTags.Remove(invalidTagsPair);
                    if (invalidTagsPair.StartPosition > currentPair.StartPosition) continue;
                    InsertOpeningTags(openingTags, invalidTagsPair.StartTag);
                }
                if (CanRemoveCurrentPairInTheFuture(openingTags, currentSpecifications, currentPair.PairType))
                    tagsAfterBlockingTags.Add(currentPair);
                else
                    previousPairs.Add(currentPair);
            }
            previousPairs.AddRange(tagsAfterBlockingTags);
            return previousPairs;
        }

        private static bool CurrentTagCannotCreatePair(Tag startTag, Tag currentTag)
        {
            return startTag == null || currentTag.PositionType == PositionType.OpeningTag;
        }

        private static void InsertOpeningTags(List<Tag> openingTags, Tag tagForAdd)
        {
            var indexForInsert = openingTags.FindIndex(0, tag => tag.PositionInText > tagForAdd.PositionInText);
            if (indexForInsert == -1)
                openingTags.Add(tagForAdd);
            else
                openingTags.Insert(indexForInsert, tagForAdd);
        }

        private static bool CanRemoveCurrentPairInTheFuture(IReadOnlyCollection<Tag> openingTags, IReadOnlyCollection<TagSpecification> currentSpecifications, TagType typeCurrentPair)
        {
            return openingTags.FirstOrDefault(tag =>
                currentSpecifications.First(ts => ts.TagType == tag.TagType).IgnoreTags
                    .Contains(typeCurrentPair)) != null;
        }

        private static IEnumerable<Tag> GetAllInvalidOpeningTag(TagsPair currentPair, IReadOnlyCollection<Tag> previousOpeningTags, IReadOnlyCollection<TagSpecification> currentSpecifications)
        {
            var specificationForCurrentPairType = currentSpecifications.First(t => t.TagType == currentPair.PairType);
            return previousOpeningTags.Where(tag => tag.PositionInText > currentPair.StartPosition).Where(tag => specificationForCurrentPairType.IgnoreTags.Contains(tag.TagType)).ToList();
        }

        private static bool TryGetLastLineOpenTag(IReadOnlyCollection<Tag> openingTags, IReadOnlyCollection<TagType> lineSpecifications, Tag currentTag, out Tag newTag)
        {
            newTag = null;
            var openingLine = openingTags.LastOrDefault(t => lineSpecifications.Contains(t.TagType));
            if (openingLine == null)
                return false;
            newTag = new Tag(openingLine.TagType, currentTag.PositionInText,
                PositionType.ClosingTag, currentTag.Value);
            return true;
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
            return !char.IsWhiteSpace(text[position + symbol.Length]);
        }

        private static bool CanBeEndTag(string symbol, int position, StringBuilder text)
        {
            if (position == 0)
            {
                return false;
            }
            return !char.IsWhiteSpace(text[position - 1]);
        }
    }
}
