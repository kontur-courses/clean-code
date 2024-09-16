namespace Markdown.Tags
{
    public class HeaderTag : ITag
    {
        public TagType Type { get; } = TagType.Header;
        public int Position { get; }
        public bool IsEndTag { get; }

        public HeaderTag(int position, bool isEndTag = false)
        {
            Position = position;
            IsEndTag = isEndTag;
        }
    }
}
