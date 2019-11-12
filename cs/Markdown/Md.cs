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

        private const char EscapeSymbol = '\\';
        public static string Render(string sourceText)
        {
            var currentText = new StringBuilder(sourceText);
            var tagsSequences = GetTagsSequences(currentText, MdTags.GetAllTags());
            var tagsPair = GetTagsPair(tagsSequences);
            var result = ReplaceTags(currentText, MdTags.GetAllTags(), HTMLTags.GetAllSpecifications(), tagsPair);
            return result;
        }

        private static Queue<Tag> GetTagsSequences(StringBuilder sourceString, List<TagSpecification> currentSpecifications)
        {
            var result = new Queue<Tag>();
            var isEscaped = false;
            for (var currentPosition = 0; currentPosition < sourceString.Length; currentPosition++)
            {
                if (EscapeSymbol == sourceString[currentPosition])
                {
                    if (isEscaped)
                    {
                        isEscaped = false;
                        continue;
                    }
                    isEscaped = true;
                    sourceString.Remove(currentPosition, 1);
                    currentPosition -= 1;
                    continue;
                }
                var possibleStartTag = GetAllPossibleStartTag(currentSpecifications, sourceString, currentPosition).FirstOrDefault();
                var possibleEndTag = GetAllPossibleEndTag(currentSpecifications, sourceString, currentPosition).FirstOrDefault();
                if (possibleEndTag == null && possibleStartTag == null)
                {
                    isEscaped = false;
                    continue;
                }
                var priorityEnd = possibleEndTag == null ? int.MaxValue : currentSpecifications.FindIndex(0,
                    specification => specification.TagType == possibleEndTag.TagType);
                var priorityStart = possibleStartTag == null ? int.MaxValue : currentSpecifications.FindIndex(0,
                    specification => specification.TagType == possibleStartTag.TagType);
                var positionType = priorityStart == priorityEnd ? PositionType.any : priorityStart < priorityEnd ? PositionType.OpeningTag : PositionType.ClosingTag;
                var tagType = priorityStart <= priorityEnd ? possibleStartTag.TagType : possibleEndTag.TagType;
                var tag = new Tag(tagType, currentPosition, positionType);
                if (!isEscaped)
                    result.Enqueue(tag);
                currentPosition += (priorityStart <= priorityEnd) ? possibleStartTag.StartTag.Length - 1 : possibleEndTag.EndTag.Length - 1;
                isEscaped = false;
            }
            return result;
        }

        private static List<TagSpecification> GetAllPossibleStartTag(List<TagSpecification> currentSpecifications,
            StringBuilder sourceString, int currentPosition)
        {
            return currentSpecifications.FindAll(tag => tag.StartTag.StartsWith(sourceString[currentPosition]))
                .Where(possibleTag => IsValidTag(sourceString, possibleTag.StartTag, currentPosition, CanBeOpenTag))
                .ToList();
        }

        private static List<TagSpecification> GetAllPossibleEndTag(List<TagSpecification> currentSpecifications,
            StringBuilder sourceString, int currentPosition)
        {
            return currentSpecifications.FindAll(tag => tag.EndTag.StartsWith(sourceString[currentPosition]))
                .Where(possibleTag => IsValidTag(sourceString, possibleTag.StartTag, currentPosition, CanBeEndTag))
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

        private static List<TagsPair> GetTagsPair(Queue<Tag> characterSequences)
        {
            var previousPairs = new List<TagsPair>();
            var openingTags = new List<Tag>();
            while (characterSequences.Count > 0)
            {
                var currentTag = characterSequences.Dequeue();
                if (currentTag.positionType == PositionType.OpeningTag)
                {
                    openingTags.Add(currentTag);
                    continue;
                }
                var startTag = openingTags.FindLast(t => t.TagType == currentTag.TagType);
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
                ValidatePair(currentPair, previousPairs, openingTags);
            }
            return previousPairs;
        }

        private static void ValidatePair(TagsPair currentPair, List<TagsPair> previousPairs, List<Tag> previousOpeningTags)
        {
            var tagsInsideCurrentPair = previousOpeningTags.Where(tag => tag.PositionInText > currentPair.StartPosition).ToList();
            var specificationForCurrentPairType = MdTags.GetAllTags().First(t => t.TagType == currentPair.PairType);
            foreach (var tag in tagsInsideCurrentPair)
            {
                if (specificationForCurrentPairType.IgnoreTags.Contains(tag.TagType))
                    previousOpeningTags.Remove(tag);
            }
            var intersectsWithPrevious = previousPairs.Where(pair => pair.EndPosition > currentPair.StartPosition).ToList();
            foreach (var tagsPair in intersectsWithPrevious)
            {
                if (!specificationForCurrentPairType.IgnoreTags.Contains(tagsPair.PairType))
                    continue;
                previousPairs.Remove(tagsPair);
                if (tagsPair.StartPosition > currentPair.StartPosition) continue;
                var tagForAdd = new Tag(tagsPair.PairType, tagsPair.StartPosition, PositionType.OpeningTag);
                var indexForInsert = previousOpeningTags.FindIndex(0, t => t.PositionInText > tagsPair.StartPosition);
                previousOpeningTags.Insert(indexForInsert == -1 ? 0 : indexForInsert, tagForAdd);
            }
            previousPairs.Add(currentPair);
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

        private static string ReplaceTags(StringBuilder sourceStrings, List<TagSpecification> currentSpecifications,
            List<TagSpecification> newSpecifications, List<TagsPair> tagsPair)
        {
            var delta = 0;
            var sortedTags = new SortedList<int, Tag>();
            foreach (var pair in tagsPair)
            {
                sortedTags.Add(pair.StartPosition, new Tag(pair.PairType, pair.StartPosition, PositionType.OpeningTag));
                sortedTags.Add(pair.EndPosition, new Tag(pair.PairType, pair.EndPosition, PositionType.ClosingTag));
            }
            foreach (var tag in sortedTags.Values)
            {
                var tagForReplace = currentSpecifications.First(t => t.TagType == tag.TagType);
                var oldTag = tag.positionType == PositionType.OpeningTag ? tagForReplace.StartTag : tagForReplace.EndTag;
                var newTags = newSpecifications.First(t => t.TagType == tag.TagType);
                var newTag = tag.positionType == PositionType.OpeningTag ? newTags.StartTag : newTags.EndTag;
                sourceStrings.Replace(oldTag, newTag, tag.PositionInText + delta, oldTag.Length);
                delta += newTag.Length - oldTag.Length;
            }
            return sourceStrings.ToString();
        }
    }
}
