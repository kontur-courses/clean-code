using System.Collections.Generic;


namespace Markdown
{
    public class Tag
    {
        public TagType Type { get; }
        public int Position { get; }


        public Tag(TagType type, int position)
        {
            Type = type;
            Position = position;
        }

        public bool IsClosing()
        {
            return CloseToOpen.ContainsKey(Type);
        }


        public TagType Opposite()
        {
            return IsClosing() ? CloseToOpen[Type] : OpenToClose[Type];
        }

        public static bool IsClosing(TagType type)
        {
            return CloseToOpen.ContainsKey(type);
        }

        public static TagType Closing(TagType type)
        {
            return IsClosing(type) ? type : OpenToClose[type];
        }

        public static TagType Opposite(TagType type)
        {
            return IsClosing(type) ? CloseToOpen[type] : OpenToClose[type];
        }

        public static readonly Dictionary<TagType, TagType> CloseToOpen = new Dictionary<TagType, TagType>
        {
            {TagType.EmClose, TagType.Em },
            {TagType.AClose, TagType.A },
            {TagType.StrongClose, TagType.Strong },
            {TagType.SClose, TagType.S},
            {TagType.LinkBracketClose, TagType.LinkBracket}
        };

        public static readonly Dictionary<TagType, TagType> OpenToClose = new Dictionary<TagType, TagType>
        {
            {TagType.Em, TagType.EmClose },
            {TagType.A, TagType.AClose },
            {TagType.Strong, TagType.StrongClose },
            {TagType.S, TagType.SClose},
            {TagType.LinkBracket, TagType.LinkBracketClose }
        };

        public static readonly Dictionary<TagType, string> TagTextRepresentation = new Dictionary<TagType, string>
        {
            {TagType.Em, "<em>" },
            {TagType.EmClose, "</em>" },
            {TagType.Strong, "<strong>" },
            {TagType.StrongClose, "</strong>" },
            {TagType.A, "<a>" },
            {TagType.AClose, "</a>" },
            {TagType.S, "<s>" },
            {TagType.SClose, "</s>" }
        };

        public static readonly Dictionary<TagType, string> TagMarkerTextRepresentation = new Dictionary<TagType, string>
        {
            {TagType.Em, "_" },
            {TagType.EmClose, "_" },
            {TagType.Strong, "__" },
            {TagType.StrongClose, "__" },
            {TagType.A, "[" },
            {TagType.AClose, "]" },
            {TagType.S, "~~" },
            {TagType.SClose, "~~" },
            {TagType.Backslash, @"\" },
            {TagType.LinkBracket, "(" },
            {TagType.LinkBracketClose, ")" }
        };


        public static readonly Dictionary<TagType, HashSet<TagType>> ProhibitedOuterTags = new Dictionary<TagType, HashSet<TagType>>
        {
            { TagType.Strong, new HashSet<TagType> { TagType.Em }}
        };
    }

    public enum TagType
    {
        Em,
        EmClose,
        Strong,
        StrongClose,
        Backslash,
        A,
        AClose,
        LinkBracket,
        LinkBracketClose,
        S,
        SClose

    }
}
