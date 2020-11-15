using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown.Tag
{
    public class TagData : ITagData
    {
        public FormattingState State { get; }
        public TagBorder IncomingBorder { get; }
        public TagBorder OutgoingBorder { get; }
        
        private readonly HashSet<FormattingState> notAllowedNestedStates;

        public TagData(FormattingState state, TagBorder incomingBorder, TagBorder outgoingBorder,
            params FormattingState[] notAllowedNestedStates)
        {
            State = state;
            IncomingBorder = incomingBorder;
            OutgoingBorder = outgoingBorder;
            this.notAllowedNestedStates = notAllowedNestedStates.ToHashSet();
        }

        public virtual bool IsValid(string data, int startPos, int endPos)
        {
            return true;
        }

        public bool CanNested(FormattingState stateToNesting)
        {
            return !notAllowedNestedStates.Contains(stateToNesting);
        }
    }
}