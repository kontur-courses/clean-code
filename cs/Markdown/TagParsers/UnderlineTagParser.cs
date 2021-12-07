using System.Collections.Generic;
using Markdown.TagEvents;

namespace Markdown.TagParsers
{
    public class UnderlineTagParser : ITagParser
    {
        private readonly TagName currentUnderliner;
        private readonly List<TagEvent> tagEvents;
        private State state;

        public UnderlineTagParser(List<TagEvent> tagEvents, TagName currentUnderliner)
        {
            this.tagEvents = tagEvents;
            this.currentUnderliner = currentUnderliner;
            this.state = State.Start;
        }

        public List<TagEvent> Parse()
        {
            for (var tagIndex = 0; tagIndex < tagEvents.Count; tagIndex++)
                ChangeTagAccordingToState(tagEvents[tagIndex], tagIndex);
            return tagEvents;
        }

        private void ChangeTagAccordingToState(TagEvent tagEvent, int tagIndex)
        {
            switch (state)
            {
                case State.Start:
                    ProcessStartState(tagEvent);
                    break;
                case State.FindClosingUnderline:
                    ProcessStateFindClosingUnderline(tagEvent);
                    break;
                case State.CheckClosingContext:
                    ProcessStateCheckClosingContext(tagEvent, tagIndex);
                    break;
                case State.NewLine:
                case State.Whitespace:
                    ProcessStateWhitespace(tagEvent);
                    break;
                case State.Other:
                    ProcessStateOther(tagEvent);
                    break;
            }
        }

        private void ProcessStartState(TagEvent tagEvent)
        {
            if (tagEvent.Name == currentUnderliner)
            {
                ChangeUnderlinerSideAndState(tagEvent);
            }
            else
                MakeStandartStateChanging(tagEvent);
        }

        private void ProcessStateFindClosingUnderline(TagEvent tagEvent)
        {
            if (tagEvent.Name == currentUnderliner)
            {
                state = State.CheckClosingContext;
            }
            switch (tagEvent.Name)
            {
                case TagName.Whitespace:
                    state = State.Whitespace;
                    break;
                case TagName.NewLine:
                    state = State.NewLine;
                    break;
            }
        }

        private void ProcessStateCheckClosingContext(TagEvent currentTag, int tagIndex)
        {
            var tagBeforeClosingTag = tagEvents[tagIndex - 2];
            var closingTag = tagEvents[tagIndex - 1];
            if (IsClosingContextNumbersOnly(currentTag, tagBeforeClosingTag))
            {
                ConvertUnderlinerToWord(closingTag);
                state = State.FindClosingUnderline;
            }
            else
            {
                closingTag.Side = TagSide.Right;
                MakeStandartStateChanging(currentTag);
            }
        }

        private void ProcessStateWhitespace(TagEvent tagEvent)
        {
            if (tagEvent.Name == currentUnderliner)
                ChangeUnderlinerSideAndState(tagEvent);
            else
                MakeStandartStateChanging(tagEvent);
        }

        private void ProcessStateOther(TagEvent tagEvent)
        {
            if (tagEvent.Name == currentUnderliner)
                ChangeUnderlinerSideAndState(tagEvent);
            else 
                MakeStandartStateChanging(tagEvent);
        }

        private void MakeStandartStateChanging(TagEvent tagEvent)
        {
            switch (tagEvent.Name)
            {
                case TagName.Whitespace:
                    state = State.Whitespace;
                    break;
                case TagName.NewLine:
                    state = State.NewLine;
                    break;
                default:
                    state = State.Other;
                    break;
            }
        }

        private void ChangeUnderlinerSideAndState(TagEvent tagEvent)
        {
            state = State.FindClosingUnderline;
            tagEvent.Side = TagSide.Left;
        }


        private void ConvertUnderlinerToWord(TagEvent tagEvent)
        {
            tagEvent.Name = TagName.Word;
            tagEvent.Side = TagSide.None;
        }

        private bool IsClosingContextNumbersOnly(TagEvent currentTag, TagEvent tagBeforeClosingUnderline)
        {
            return tagBeforeClosingUnderline.Name == TagName.Number 
                   && currentTag.Name == TagName.Number;
        }

        private TagName GetAnotherUnderlineTagKind(TagName parsingName)
        {
            if (parsingName == TagName.DoubleUnderliner) 
                return TagName.Underliner;
            return TagName.DoubleUnderliner;
        }
    }
}
