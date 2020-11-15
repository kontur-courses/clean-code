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
        
        public override bool IsValid(string data, int startPos, int endPos)
        {
            throw new NotImplementedException();
            var isOpenValid = startPos == 0 || data[startPos - 1] == '\n';
            var isCloseValid = startPos == 0 || data[startPos - 1] == '\n';
            return isOpenValid && isCloseValid;
        }
    }
}