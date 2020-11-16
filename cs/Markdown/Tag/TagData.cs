using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown.Tag
{
    public class TagData : ITagData
    {
        public TagBorder IncomingBorder { get; }
        public TagBorder OutgoingBorder { get; }
        public EndOfLineAction AtLineEndAction { get; }

        private readonly HashSet<ITagData> notAllowedNestedTags;

        public TagData(TagBorder incomingBorder, TagBorder outgoingBorder,
            EndOfLineAction endOfLineAction,
            params ITagData[] notAllowedNestedTags)
        {
            AtLineEndAction = endOfLineAction;
            IncomingBorder = incomingBorder;
            OutgoingBorder = outgoingBorder;
            this.notAllowedNestedTags = notAllowedNestedTags.ToHashSet();
        }

        public virtual bool IsValid(string data, int startPos, int endPos)
        {
            return true;
        }

        public bool CanNested(ITagData stateToNesting)
        {
            return !notAllowedNestedTags.Contains(stateToNesting);
        }
    }
}