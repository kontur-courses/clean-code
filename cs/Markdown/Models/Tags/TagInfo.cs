namespace Markdown.Models.Tags
{
    internal class TagInfo
    {
        public Tag Tag { get; }
        public int Position { get; }
        public int TagLength { get; }
        public bool IsEscaped { get; }

        public TagInfo(Tag tag, int position, int tagLength, bool isEscaped = false)
        {
            Tag = tag;
            Position = position;
            TagLength = tagLength;
            IsEscaped = isEscaped;
        }
    }
}
