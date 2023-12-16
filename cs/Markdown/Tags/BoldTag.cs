namespace Markdown.Tags
{
    public class BoldTag : ITag
    {
        public TagType Type { get; } = TagType.Bold;
        public int Position { get; }
        public bool IsEndTag { get; }

        public BoldTag(int position, bool isEndTag = false)
        {
            Position = position;
            IsEndTag = isEndTag;
        }
    }
}
