using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class TagData : ITagData
    {
        public FormattingState State { get; }
        public TagBorder IncomingBorder { get; }
        public TagBorder OutgoingBorder { get; }
        public readonly HashSet<FormattingState> NotAllowedNestedStates;

        public TagData(FormattingState state, TagBorder incomingBorder, TagBorder outgoingBorder,
            params FormattingState[] notAllowedNestedStates)
        {
            State = state;
            IncomingBorder = incomingBorder;
            OutgoingBorder = outgoingBorder;
            NotAllowedNestedStates = notAllowedNestedStates.ToHashSet();
        }
    }
}