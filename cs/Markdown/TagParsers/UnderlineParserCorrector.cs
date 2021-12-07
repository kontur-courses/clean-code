using System.Collections.Generic;
using Markdown.TagEvents;

namespace Markdown.TagParsers
{
    public class UnderlineParserCorrector : ITagParser
    {
        private readonly List<TagEvent> tagEvents;

        public UnderlineParserCorrector(List<TagEvent> tagEvents)
        {
            this.tagEvents = tagEvents;
        }

        public List<TagEvent> Parse()
        {
            for (var tagIndex = 0; tagIndex < tagEvents.Count; tagIndex++)
            {
                if (tagEvents[tagIndex].IsUnderliner())
                    TryToCorrectTagEvent(tagIndex);
            }

            return tagEvents;
        }

        public void TryToCorrectTagEvent(int tagIndex)
        {
            var currentTag = tagEvents[tagIndex];
            if (IsUnderlinerOnTheEndOfTheWord(tagIndex))
                currentTag.Side = TagSide.Right;
            if (IsUnderlinerSituatedSeparately(tagIndex))
            {
                currentTag.Side = TagSide.None;
                currentTag.Name = TagName.Word;
            }
        }

        private bool IsUnderlinerSituatedSeparately(int tagIndex)
        {
            var tag = tagEvents[tagIndex];
            var prevTag = GetTagOrNull(tagIndex - 1);
            var nextTag = GetTagOrNull(tagIndex + 1);
            return IsTagBetweenWhiteSpaces(prevTag, nextTag)
                     || IsTagOnTheLeftEdge(prevTag, nextTag)
                     || IsTagOnTheRightEdge(prevTag, nextTag)
                     || IsTagTheOnlyOne(prevTag, nextTag);
        }

        private static bool IsTagOnTheRightEdge(TagEvent prevTag, TagEvent nextTag)
        {
            return prevTag != null && prevTag.IsWhiteSpaceOrNewLineOrEof() && nextTag == null;
        }

        private static bool IsTagOnTheLeftEdge(TagEvent prevTag, TagEvent nextTag)
        {
            return prevTag == null && nextTag != null && nextTag.IsWhiteSpaceOrNewLineOrEof();
        }

        private static bool IsTagBetweenWhiteSpaces(TagEvent prevTag, TagEvent nextTag)
        {
            return prevTag != null && nextTag != null
                                   && prevTag.IsWhiteSpaceOrNewLineOrEof()
                                   && nextTag.IsWhiteSpaceOrNewLineOrEof();
        }

        private static bool IsTagTheOnlyOne(TagEvent prevTag, TagEvent nextTag)
        {
            return prevTag == null && nextTag == null;
        }

        private static void ConvertTagNameToWord(TagEvent tag)
        {
            tag.Name = TagName.Word;
            tag.Side = TagSide.None;
        }

        private bool IsUnderlinerOnTheEndOfTheWord(int tagIndex)
        {
            var prevIndex = tagIndex - 1;
            var prevTag = GetTagOrNull(prevIndex);
            var nextTag = tagEvents[tagIndex + 1];
            return prevIndex >= 0 
                   && prevTag.IsWordOrNumber() 
                   && nextTag.IsWhiteSpaceOrNewLineOrEof();
        }

        private TagEvent GetTagOrNull(int tagInd)
        {
            return tagInd >= 0 && tagInd < tagEvents.Count
                ? tagEvents[tagInd]
                : null;
        }
    }
}
