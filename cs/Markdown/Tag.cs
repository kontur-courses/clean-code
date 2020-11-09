using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class Tag
    {
        private static readonly Dictionary<TagType, TagInfo> TagInfos = new Dictionary<TagType, TagInfo>
        {
            [TagType.Em] = new TagInfo("_", 1, "<em>"),
            [TagType.H1] = new TagInfo("#", 1, "<h1>"),
            [TagType.Shield] = new TagInfo("\\", 1, ""),
            [TagType.Strong] = new TagInfo("__", 2, "<strong>"),
            [TagType.Reference] = new TagInfo("[", 1, ""),
        };

        public static readonly HashSet<char> MdFirstChars = TagInfos.Values.Select(info => info.Md[0]).ToHashSet();

        public readonly TagType Type;
        public readonly int Start;
        public int Length { get; protected set; }
        protected bool IsOpening;

        protected Tag(TagType type, int start, bool isOpening = true)
        {
            Type = type;
            Start = start;
            Length = TagInfos[type].Length;
            IsOpening = isOpening;
        }

        public virtual string ToHtml()
        {
            var html = TagInfos[Type].Html;
            return IsOpening ? html : html.Insert(1, "/");
        }
    }
}