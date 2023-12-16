namespace Markdown.Tags
{
    public class BoldTag : ITag
    {
        public TagType Type { get; } = TagType.Bold;
        public string StartTag { get; } = "__";
        public string EndTag { get; } = "__";
        public int Position { get; }
        
        public BoldTag(int position)
        {
            Position = position;
        }
    }
}
