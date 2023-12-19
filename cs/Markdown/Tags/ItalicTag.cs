namespace Markdown.Tags
{
    public class ItalicTag : ITag
    {
        public TagType Type { get; } = TagType.Italic;
        public int Position { get; }
        public bool IsEndTag { get; }
        public bool IsShielded { get; private set; }

        public ItalicTag(int position, bool isEndTag = false)
        {
            Position = position;
            IsEndTag = isEndTag;
        }
        
        public void ShieldTag() => IsShielded = true;
    }
}
