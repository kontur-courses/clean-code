namespace Markdown.Tags
{
    public class BulletedListTag : ITag
    {
        public TagType Type { get; } = TagType.BulletedList;
        public int Position { get; }
        public bool IsEndTag { get; }

        public BulletedListTag(int position, bool isEndTag = false)
        {
            Position = position;
            IsEndTag = isEndTag;
        }
    }
}
