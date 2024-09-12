namespace MarkdownTask
{
    public class Token
    {
        public readonly int Position;
        public readonly TagInfo.TagType TagType;
        public readonly TagInfo.Tag Tag;
        public readonly int TagLength;

        public Token(TagInfo.TagType tagType, int position, TagInfo.Tag tag, int tagLength)
        {
            Position = position;
            TagType = tagType;
            Tag = tag;
            TagLength = tagLength;
        }
    }
}