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
        public virtual bool IsBreaksWhenNestedNotComplete => false;
        public virtual ITagData ParentTag { get; }

        private readonly HashSet<ITagData> notAllowedNestedTags;

        public TagData(TagBorder incomingBorder, TagBorder outgoingBorder,
            EndOfLineAction endOfLineAction,
            ITagData requiredParentTag=null,
            params ITagData[] notAllowedNestedTags)
        {
            AtLineEndAction = endOfLineAction;
            IncomingBorder = incomingBorder;
            OutgoingBorder = outgoingBorder;
            ParentTag = requiredParentTag;
            this.notAllowedNestedTags = notAllowedNestedTags.ToHashSet();
        }

        public virtual bool IsValidAtClose(string data, int startPos, int endPos)
        {
            return true;
        }
        
        public virtual bool IsValidAtOpen(string data, int startPos)
        {
            return true;
        }

        public bool CanNested(ITagData tagToNesting)
        {
            return !notAllowedNestedTags.Contains(tagToNesting);
        }
    }
}