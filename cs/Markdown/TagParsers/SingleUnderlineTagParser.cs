using System.Collections.Generic;
using Markdown.TagEvents;

namespace Markdown.TagParsers
{
    public class SingleUnderlineTagParser : ITagParser
    {
        private readonly List<TagEvent> tagEvents;
        private State state;

        public SingleUnderlineTagParser(List<TagEvent> tagEvents)
        {
            this.tagEvents = tagEvents;
            this.state = State.Start;
        }

        public List<TagEvent> Parse()
        {
            for (var tagIndex = 0; tagIndex < tagEvents.Count; tagIndex++)
                ChangeTagAccordingToState(tagIndex);
            return tagEvents;
        }

        private void ChangeTagAccordingToState(int tagIndex)
        {
            switch (state)
            {
                //case State.Start: ProcessStartState(tagIndex);
                //    break;
        }
    }

        private void ProccessStartState(int tagIndex)
        {
            var tagEvent = tagEvents[tagIndex];
            switch (tagEvent.Name)
            {

            }
        }
    }
}
