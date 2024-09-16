namespace Markdown.Tags
{
    public class ItalicTag : ITag
    {
        public TagType Type { get; } = TagType.Italic;
        public int Position { get; }
        public bool IsEndTag { get; }

        public ItalicTag(int position, bool isEndTag = false)
        {
            Position = position;
            IsEndTag = isEndTag;
        }
    }
}
