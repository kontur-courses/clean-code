using System;

namespace Markdown.MdTags
{
    public static class TagBuilder
    {
        public static Tag OfType(TagType type)
        {
            return OfType(type, 0, 0);
        }

        public static Tag WithBounds(this Tag tag, int start, int end)
        {
            //start и end в этом методе - индекс последнего ограничивающего тег символа (самого правого)
            return tag.Type == TagType.StrongText ? OfType(tag.Type, start - 1, end) : OfType(tag.Type, start, end);
        }

        private static Tag OfType(TagType type, int start, int end)
        {
            return type switch
            {
                TagType.None => throw new ArgumentException(),
                TagType.Title => new TitleTag(start, end),
                TagType.StrongText => new StrongTextTag(start, end),
                TagType.UnnumberedList => new UnnumberedList(start, end),
                TagType.ListElement => new ListElement(start, end),
                _ => new ItalicsTag(start, end)
            };
        }
    }
}