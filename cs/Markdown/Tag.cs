using System.Collections.Generic;

namespace Markdown
{
    public class Tag
    {
        private static readonly Dictionary<TagType, int> LengthOf = new Dictionary<TagType, int>
        {
            [TagType.Em] = 1,
            [TagType.H1] = 1,
            [TagType.Strong] = 2,
            [TagType.Shield] = 1,
        };

        private static readonly Dictionary<TagType, string> MdToHtml = new Dictionary<TagType, string>
        {
            [TagType.Em] = "<em>",
            [TagType.H1] = "<h1>",
            [TagType.Strong] = "<strong>",
            [TagType.Shield] = "",
        };

        public readonly TagType Type;
        public readonly int Start;
        public readonly int Length;
        public bool IsOpening = true;

        public Tag(TagType type, int start)
        {
            Type = type;
            Start = start;
            Length = LengthOf[type];
        }

        public string ToHtml()
        {
            var html = MdToHtml[Type];
            return IsOpening ? html : html.Insert(1, "/");
        }

        private bool IsTagInMiddleOfWord(string text)
        {
            return false;
        }

        private bool IsTagsInSameWord(Tag second, string text)
        {
            return false;
        }

        public bool IsCorrectTagPair(Tag other, string text)
        {
            return true;
        }
    }
}