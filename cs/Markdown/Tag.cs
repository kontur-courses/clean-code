using System.Collections.Generic;

namespace Markdown
{
    public class Tag
    {
        protected static readonly Dictionary<TagType, TagInfo> TagInfos = new Dictionary<TagType, TagInfo>
        {
            [TagType.Em] = new TagInfo(1, "<em>", true),
            [TagType.H1] = new TagInfo(1, "<h1>", false),
            [TagType.Shield] = new TagInfo(1, "", false),
            [TagType.Strong] = new TagInfo(2, "<strong>",true),
        };

        public readonly TagType Type;
        public readonly int Start;
        public readonly int Length;

        public Tag(TagType type, int start)
        {
            Type = type;
            Start = start;
            Length = TagInfos[type].Length;
        }

        public static Tag CreateTag(TagType type, int start)
        {
            if (TagInfos[type].IsPaired) return new PairedTag(type, start);
            return new SingleTag(type, start);
        }

        public virtual string ToHtml()
        {
            return TagInfos[Type].Html;
        }
    }
}