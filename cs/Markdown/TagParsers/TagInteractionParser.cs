using System.Collections.Generic;
using System.Data;
using Markdown.TagEvents;
using NUnit.Framework;

namespace Markdown.TagParsers
{
    public class TagInteractionParser : ITagParser
    {
        private readonly List<TagEvent> tagEvents;
        private readonly Stack<TagEvent> tagPairs;

        public TagInteractionParser(List<TagEvent> tagEvents)
        {
            this.tagEvents = tagEvents;
            tagPairs = new Stack<TagEvent>();
        }
        public List<TagEvent> Parse()
        {
            for (var tagIndex = 0; tagIndex < tagEvents.Count; tagIndex++)
                CheckTagInteraction(tagIndex);
            return tagEvents;
        }

        public void CheckTagInteraction(int tagIndex)
        {
            var tagEvent = tagEvents[tagIndex];
            var tagName = tagEvent.Name;
            if (tagName == TagName.Header)
                ProcessHeader(tagEvent);
            else if (tagName == TagName.Underliner)
                ProcessUnderliner(tagEvent);
            else if (tagName == TagName.DoubleUnderliner)
                ProcessDoubleUnderliner(tagEvent);
            else if (tagName == TagName.NewLine)
                ProcessNewLine(tagEvent);
            else if (tagName == TagName.Eof)
                ProcessEof(tagEvent);
        }

        private void ProcessHeader(TagEvent tagEvent)
        {
            if (tagPairs.Count == 0)
                tagPairs.Push(tagEvent);
        }

        private void ProcessUnderliner(TagEvent tagEvent)
        {
            if (tagEvent.HasNoSide())
                tagEvent.Name = TagName.Word;
            else if (tagEvent.HasLeftSide())
            {
                if (IsTagStackEmpty() || IsHeaderOnStackTop() || IsLeftDoubleUnderlinerOnStackTop())
                    tagPairs.Push(tagEvent);
                else if (IsLeftUnderlinerOnStackTop())
                    tagEvent.ConvertToWord();
            }
            else if (tagEvent.HasRightSide())
            {
                if (IsTagStackEmpty() || IsHeaderOnStackTop())
                    tagEvent.ConvertToWord();
                else if (IsLeftUnderlinerOnStackTop())
                    tagPairs.Pop();
                else if (IsLeftDoubleUnderlinerOnStackTop())
                {
                    var leftDoubleUnderliner = tagPairs.Pop();
                    if (!IsTagStackEmpty() && IsLeftUnderlinerOnStackTop())
                    {
                        // для случая _ __ _ __
                        tagPairs.Push(leftDoubleUnderliner);
                        tagPairs.Push(tagEvent);
                    }
                    else
                    {
                        //для случая __ _
                        leftDoubleUnderliner.ConvertToWord();
                        tagEvent.ConvertToWord();
                    }
                }
            }
        }

        private void ProcessDoubleUnderliner(TagEvent tagEvent)
        {
            if (tagEvent.HasNoSide())
                tagEvent.ConvertToWord();
            else if (tagEvent.HasLeftSide())
            {
                if (IsTagStackEmpty() || IsHeaderOnStackTop())
                    tagPairs.Push(tagEvent);
                else if (IsLeftDoubleUnderlinerOnStackTop())
                    tagEvent.ConvertToWord();
                else if (IsLeftUnderlinerOnStackTop())
                    tagPairs.Push(tagEvent);
            }
            else if (tagEvent.HasRightSide())
            {
                if (IsTagStackEmpty() || IsHeaderOnStackTop())
                    tagEvent.ConvertToWord();
                else if (IsLeftUnderlinerOnStackTop())
                {
                    var stackTop = tagPairs.Pop();
                    stackTop.ConvertToWord();
                }
                else if (IsLeftDoubleUnderlinerOnStackTop())
                {
                    var stackTopDoubleUnderliner = tagPairs.Pop();
                    if (!IsTagStackEmpty() && IsLeftUnderlinerOnStackTop())
                    {
                        stackTopDoubleUnderliner.ConvertToWord();
                        tagEvent.ConvertToWord();
                    }
                }
                else if (IsRightUnderlinerOnStackTop())
                {
                    PopThreeItemsFromStackAndConvertToWord();
                }
            }
        }

        private void PopThreeItemsFromStackAndConvertToWord()
        {
            var tagsToPopCount = 3;
            while (tagsToPopCount > 0)
            {
                var tagToConvertToWord = tagPairs.Pop();
                tagToConvertToWord.ConvertToWord();
                tagsToPopCount--;
            }
        }

        private void ProcessNewLine(TagEvent tagEvent)
        {
            while (!IsTagStackEmpty())
            {
                var stackTop = tagPairs.Pop();
                if (stackTop.IsHeader())
                    continue;
                stackTop.ConvertToWord();
            }
        }

        private void ProcessEof(TagEvent tagEvent)
        {
            if (IsTagStackEmpty()) 
                return;
            ProcessNewLine(tagEvent);
            tagEvent.ConvertToRightHeader();
        }

        private void PopAllExceptHeader()
        {
            while (!IsTagStackEmpty())
            {

            }
        }

        private bool IsRightUnderlinerOnStackTop()
        {
            return tagPairs.Peek().Name == TagName.Underliner && tagPairs.Peek().Side == TagSide.Right;
        }

        private bool IsLeftUnderlinerOnStackTop()
        {
            return tagPairs.Peek().Name == TagName.Underliner && tagPairs.Peek().Side == TagSide.Left;
        }

        private bool IsLeftDoubleUnderlinerOnStackTop()
        {
            return (tagPairs.Peek().Name == TagName.DoubleUnderliner
                    && tagPairs.Peek().Side == TagSide.Left);
        }

        private bool IsHeaderOnStackTop()
        {
            return tagPairs.Peek().Name == TagName.Header;
        }

        private bool IsTagStackEmpty()
        {
            return tagPairs.Count == 0;
        }
    }
}
