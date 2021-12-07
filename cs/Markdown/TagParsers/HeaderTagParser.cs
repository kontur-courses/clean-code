using System.Collections.Generic;
using Markdown.TagEvents;

namespace Markdown.TagParsers
{
    public class HeaderTagParser : ITagParser
    {
        private readonly List<TagEvent> tagEvents;
        private State state;

        public HeaderTagParser(List<TagEvent> tagEvents)
        {
            this.tagEvents = tagEvents;
            state = State.Start;
        }

        public List<TagEvent> Parse()
        {
            for (var tagIndex = 0; tagIndex < tagEvents.Count; tagIndex++)
                ChangeTagAccordingToState(tagEvents[tagIndex]);
            return tagEvents;
        }

        private void ChangeTagAccordingToState(TagEvent tagEvent)
        {
            switch (state)
            {
                case State.Start: ProcessStateStart(tagEvent);
                    break;
                case State.Header: ProcessStateHeader(tagEvent);
                    break;
                case State.NewLine: ProcessStateNewLine(tagEvent);
                    break;
                case State.Other: ProcessStateOther(tagEvent);
                    break;
            }
        }

        private void ProcessStateOther(TagEvent tagEvent)
        {
            switch (tagEvent.Name)
            {
                case TagName.Header:
                    tagEvent.Side = TagSide.None;
                    tagEvent.Name = TagName.Word;
                    break;
                case TagName.NewLine:
                    tagEvent.Side = TagSide.None;
                    tagEvent.Name = TagName.Word;
                    state = State.NewLine;
                    break;
            }
        }

        private void ProcessStateNewLine(TagEvent tagEvent)
        {
            switch (tagEvent.Name)
            {
                case TagName.Header:
                    tagEvent.Side = TagSide.Left;
                    state = State.Header;
                    break;
                case TagName.NewLine:
                    tagEvent.Side = TagSide.None;
                    tagEvent.Name = TagName.Word;
                    break;
                default:
                    state = State.Other;
                    break;
            }
        }

        private void ProcessStateStart(TagEvent tagEvent)
        {
            switch (tagEvent.Name)
            {
                case TagName.Header:
                    state = State.Header;
                    tagEvent.Side = TagSide.Left;
                    break;
                case TagName.NewLine:
                    state = State.NewLine;
                    tagEvent.Side = TagSide.None;
                    tagEvent.Name = TagName.Word;
                    break;
                default:
                    state = State.Other;
                    break;
            }
        }

        private void ProcessStateHeader(TagEvent tagEvent)
        {
            if (tagEvent.Name == TagName.NewLine)
                state = State.NewLine;
        }
    }
}
