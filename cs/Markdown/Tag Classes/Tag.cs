using System.Collections.Generic;

namespace Markdown
{
    public class Tag
    {
        public TagSide Side { get; }
        public TagKind Kind { get; }

        private static readonly Dictionary<Tag, string> TagToString =
            new Dictionary<Tag, string>
            {
                {new Tag(TagKind.Root, TagSide.None), ""},
                {new Tag(TagKind.Header, TagSide.Opening), "#"},
                {new Tag(TagKind.Header, TagSide.Closing), "\n"}
            };

        public Tag(TagKind kind, TagSide side)
        {
            Kind = kind;
            Side = side;
        }

        public static string GetStringByTag(Tag tag)
        {
            return TagToString[tag];
        }

    }
}