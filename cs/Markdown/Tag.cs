using System.Collections.Generic;

namespace Markdown
{
    public class Tag
    {
        private static readonly Dictionary<TagType, TagInfo> TagInfos = new Dictionary<TagType, TagInfo>
        {
            [TagType.Em] = new TagInfo("_", 1, "<em>", true),
            [TagType.H1] = new TagInfo("#", 1, "<h1>", false),
            [TagType.Shield] = new TagInfo("\\", 1, "", false),
            [TagType.Strong] = new TagInfo("__", 2, "<strong>",true),
        };

        public readonly TagType Type;
        public readonly int Start;
        public readonly int Length;
        protected bool IsOpening;

        protected Tag(TagType type, int start, bool isOpening = true)
        {
            Type = type;
            Start = start;
            Length = TagInfos[type].Length;
            IsOpening = isOpening;
        }

        public static Tag CreateTag(TagType type, int start)
        {
            if (TagInfos[type].IsPaired) return new PairedTag(type, start);
            return new SingleTag(type, start);
        }

        public string ToHtml()
        {
            var html = TagInfos[Type].Html;
            return IsOpening ? html : html.Insert(1, "/");
        }
    }
}