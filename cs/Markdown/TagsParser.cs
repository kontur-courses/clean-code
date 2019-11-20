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
            var lineTags = currentSpecifications.Where(tag =>  tag.EndWithLine).Select(tag => tag.TagType).ToList();
            var tagsPair = GetTagsPair(tagsSequences, currentSpecifications, lineTags);
            return tagsPair;
        }

        private static Queue<Token> GetTagsSequences(StringBuilder sourceString, List<TagSpecification> currentSpecifications)
        {
            var result = new Queue<Token>();
            for (var currentPosition = 0; currentPosition < sourceString.Length; currentPosition++)
            {
                if (EscapeSymbol == sourceString[currentPosition])
                {
                    sourceString.Remove(currentPosition, 1);
                    continue;
                }
                var possibleStartTag = GetAllPossibleTag(currentSpecifications, sourceString, currentPosition,
                    tagSpecification => tagSpecification.StartToken, CanBeOpenToken).FirstOrDefault();
                var possibleEndTag = GetAllPossibleTag(currentSpecifications, sourceString, currentPosition,
                    tagSpecification => tagSpecification.EndToken, CanBeEndToken).FirstOrDefault();
                if (possibleEndTag == null && possibleStartTag == null)
                {
                    continue;
                }
                var tag = GetMostPriorityTagInCurrentPosition(currentSpecifications, possibleStartTag, possibleEndTag,
                    currentPosition);
                result.Enqueue(tag);
                currentPosition += tag.Value.Length - 1;
            }
            return result;
        }

        private static Token GetMostPriorityTagInCurrentPosition(List<TagSpecification> currentSpecifications, TagSpecification possibleStartTag, TagSpecification possibleEndTag, int currentPosition)
        {
            var priorityEnd = possibleEndTag == null ? int.MaxValue : currentSpecifications.FindIndex(0,
                specification => specification.TagType == possibleEndTag.TagType);
            var priorityStart = possibleStartTag == null ? int.MaxValue : currentSpecifications.FindIndex(0,
                specification => specification.TagType == possibleStartTag.TagType);
            var positionType = priorityStart == priorityEnd ? PositionType.any : priorityStart < priorityEnd ? PositionType.OpeningToken : PositionType.ClosingToken;
            var tagType = priorityStart <= priorityEnd ? possibleStartTag.TagType : possibleEndTag.TagType;
            var valueTag = priorityStart <= priorityEnd ? possibleStartTag.StartToken : possibleEndTag.EndToken;
            return new Token(tagType, currentPosition, positionType, valueTag);
        }

        private static List<TagSpecification> GetAllPossibleTag(List<TagSpecification> currentSpecifications,
            StringBuilder sourceString, int currentPosition, Func<TagSpecification, string> GetNeedPositionToken, Func<string, int, StringBuilder, bool> canBeInNeedPosition)
        {
            return currentSpecifications.Where(tag => GetNeedPositionToken(tag).StartsWith(sourceString[currentPosition]))
                .Where(possibleTag => IsValidToken(sourceString, GetNeedPositionToken(possibleTag), currentPosition, canBeInNeedPosition))
                .ToList();
        }

        private static bool IsValidToken(StringBuilder sourceString, string possibleTag, int currentPosition, Func<string, int, StringBuilder, bool> validatePosition)
        {
            var lengthPossibleTag = possibleTag.Length;
            if (currentPosition + lengthPossibleTag > sourceString.Length)
            {
                return false;
            }
            return sourceString.ToString(currentPosition, lengthPossibleTag).Equals(possibleTag)
                   && validatePosition(possibleTag, currentPosition, sourceString) && !HaveDigitAround(possibleTag, currentPosition, sourceString);
        }

        private static bool HaveDigitAround(string symbol, int position, StringBuilder text)
        {
            if (position == text.Length - symbol.Length)
                return char.IsDigit(text[position - 1]);
            if (position == 0)
                return char.IsDigit(text[position + symbol.Length]);
            return char.IsDigit(text[position - 1]) || char.IsDigit(text[position + symbol.Length]);
        }

        private static List<TagsPair> GetTagsPair(Queue<Token> characterSequences, IReadOnlyCollection<TagSpecification> currentSpecifications, IReadOnlyCollection<TagType> lineSpecifications)
        {
            var previousPairs = new List<TagsPair>();
            var openingTags = new List<Token>();
            while (characterSequences.Count > 0)
            {
                var currentToken = characterSequences.Dequeue();
                if (currentToken.TagType == TagType.EndLine)
                {
                    if (GetLastLineOpenToken(openingTags, lineSpecifications, currentToken, out var newToken))
                    {
                        currentToken = newToken;
                    }
                }
                var startToken = openingTags.FindLast(t => t.TagType == currentToken.TagType);
                if (startToken == null || currentToken.PositionType == PositionType.OpeningToken)
                {
                    if (currentToken.PositionType != PositionType.ClosingToken)
                    {
                        openingTags.Add(new Token(currentToken.TagType, currentToken.PositionInText, PositionType.OpeningToken, currentToken.Value));
                    }
                    continue;
                }
                var currentPair = new TagsPair(currentToken.TagType, startToken, currentToken);
                openingTags.Remove(startToken);
                foreach (var invalidOpeningTags in GetAllInvalidOpeningToken(currentPair, openingTags, currentSpecifications))
                {
                    openingTags.Remove(invalidOpeningTags);
                }
                foreach (var invalidTagsPair in GetAllInvalidTagsPair(currentPair, previousPairs, currentSpecifications))
                {
                    previousPairs.Remove(invalidTagsPair);
                    if (invalidTagsPair.StartPosition > currentPair.StartPosition) continue;
                    var tagForAdd = invalidTagsPair.StartToken;
                    var indexForInsert = openingTags.FindIndex(0, t => t.PositionInText > invalidTagsPair.StartPosition);
                    openingTags.Insert(indexForInsert == -1 ? 0 : indexForInsert, tagForAdd);
                }
                previousPairs.Add(currentPair);
            }
            return previousPairs;
        }

        private static IEnumerable<Token> GetAllInvalidOpeningToken(TagsPair currentPair, IReadOnlyCollection<Token> previousOpeningTags, IReadOnlyCollection<TagSpecification> currentSpecifications)
        {
            var specificationForCurrentPairType = currentSpecifications.First(t => t.TagType == currentPair.PairType);
            return previousOpeningTags.Where(tag => tag.PositionInText > currentPair.StartPosition).Where(tag => specificationForCurrentPairType.IgnoreTags.Contains(tag.TagType)).ToList();
        }

        private static bool GetLastLineOpenToken(IReadOnlyCollection<Token> openingTags, IReadOnlyCollection<TagType> lineSpecifications, Token currentToken, out Token newToken)
        {
            newToken = null;
            var openingLine = openingTags.LastOrDefault(t => lineSpecifications.Contains(t.TagType));
            if (openingLine == null)
                return false;
            newToken = new Token(openingLine.TagType, currentToken.PositionInText,
                PositionType.ClosingToken, currentToken.Value);
            return true;
        }

        private static IEnumerable<TagsPair> GetAllInvalidTagsPair(TagsPair currentPair, IReadOnlyCollection<TagsPair> previousPairs,
            IReadOnlyCollection<TagSpecification> currentSpecifications)
        {
            var specificationForCurrentPairType = currentSpecifications.First(t => t.TagType == currentPair.PairType);
            return previousPairs.Where(pair => pair.EndPosition > currentPair.StartPosition).Where(tagPair => specificationForCurrentPairType.IgnoreTags.Contains(tagPair.PairType)).ToList();
        }

        private static bool CanBeOpenToken(string symbol, int position, StringBuilder text)
        {
            if (position == text.Length - symbol.Length)
            {
                return false;
            }
            return !char.IsWhiteSpace(text[position + symbol.Length]);
        }

        private static bool CanBeEndToken(string symbol, int position, StringBuilder text)
        {
            if (position == 0)
            {
                return false;
            }
            return !char.IsWhiteSpace(text[position - 1]);
        }
    }
}
