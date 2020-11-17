using System;

namespace Markdown.Tag.SpecificTags
{
    public class StyleTagData : TagData
    {
        public StyleTagData(TagBorder incomingBorder, TagBorder outgoingBorder, 
            params ITagData[] notAllowedNestedTags) 
            : base(incomingBorder, outgoingBorder, EndOfLineAction.Cancel, notAllowedNestedTags)
        { }
        

        public override bool IsValidAtOpen(string data, int startPos)
        {
            return data.Length > startPos && data[startPos + 1] != ' ';
        }

        public override bool IsValidAtClose(string data, int startPos, int endPos)
        {
            if (data[endPos - 1] == ' ')
                return false;
            return true;
        }
    }
}