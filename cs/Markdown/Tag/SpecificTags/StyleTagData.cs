using System;

namespace Markdown.Tag.SpecificTags
{
    public class StyleTagData : TagData
    {
        // очень не нравится этот конструктор
        // TODO: сделать менее громоздким
        public StyleTagData(TagBorder incomingBorder, TagBorder outgoingBorder, 
            params ITagData[] notAllowedNestedTags) 
            : base(incomingBorder, outgoingBorder, notAllowedNestedTags)
        { }
    }
}