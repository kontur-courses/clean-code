using System.Collections.Generic;


namespace Markdown
{
    public class Tag
    {
        public TagType CurrentType { get; }
        public int Position { get; }


        public Tag(TagType currentType, int position)
        {
            CurrentType = currentType;
            Position = position;
        }

        public bool IsClosing()
        {
            return CloseToOpen.ContainsKey(CurrentType);
        }


        public TagType Opposite()
        {
            return IsClosing() ? CloseToOpen[CurrentType] : OpenToClose[CurrentType];
        }

        public static bool IsClosing(TagType type)
        {
            return CloseToOpen.ContainsKey(type);
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
            {TagType.SClose, TagType.S}
        };

        public static readonly Dictionary<TagType, TagType> OpenToClose = new Dictionary<TagType, TagType>
        {
            {TagType.Em, TagType.EmClose },
            {TagType.A, TagType.AClose },
            {TagType.Strong, TagType.StrongClose },
            {TagType.S, TagType.SClose}
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
