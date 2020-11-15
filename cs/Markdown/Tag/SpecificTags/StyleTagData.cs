using System;

namespace Markdown.Tag.SpecificTags
{
    public class StyleTagData : TagData
    {
        // очень не нравится этот конструктор
        // TODO: сделать менее громоздким
        public StyleTagData(FormattingState state, 
            TagBorder incomingBorder, TagBorder outgoingBorder, 
            params FormattingState[] notAllowedNestedStates) 
            : base(state, incomingBorder, outgoingBorder, notAllowedNestedStates)
        { }
    }
}