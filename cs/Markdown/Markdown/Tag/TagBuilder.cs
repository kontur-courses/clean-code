using System;

namespace Markdown.Tag
{
    public static class TagBuilder
    {
        public static ITag OfType(TagType type)
        {
            return OfType(type, 0, 0);
        }

        public static ITag WithBounds(this ITag tag, int start, int end)
        {
            //start и end в этом методе - индекс последнего ограничивающего тег символа(самого правого)
            return tag.Type == TagType.StrongText ? OfType(tag.Type, start - 1, end) : OfType(tag.Type, start, end);
        }

        private static ITag OfType(TagType type, int start, int end)
        {
            return type switch
            {
                TagType.None => throw new ArgumentException(),
                TagType.Title => new TitleTag(start, end),
                TagType.StrongText => new StrongTextTag(start, end),
                _ => new ItalicsTag(start, end)
            };
        }
    }
}