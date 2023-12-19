namespace Markdown.Tags
{
    public class HeaderTag : ITag
    {
        public TagType Type { get; } = TagType.Header;
        public int Position { get; }
        public bool IsEndTag { get; }
        public bool IsShielded { get; private set; }

        public HeaderTag(int position, bool isEndTag = false)
        {
            Position = position;
            IsEndTag = isEndTag;
        }
        
        public void ShieldTag() => IsShielded = true;
    }
}
