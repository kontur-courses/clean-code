namespace Markdown.Tags
{
    public class HeaderTag : ITag
    {
        public  TagType Type { get; } = TagType.Header;
        public  string StartTag { get; } = "# ";
        public  string EndTag { get; } = null;
        public int Position { get; }
        
        public HeaderTag(int position)
        {
            Position = position;
        }
    }
}
