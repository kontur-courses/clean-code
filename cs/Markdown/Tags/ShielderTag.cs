namespace Markdown.Tags
{
    public class ShielderTag: ITag
    {
        public TagType Type { get; } = TagType.Shield;
        public int Position { get; }
        public bool IsEndTag { get; }
        public bool IsShielded { get; private set; }

        public ShielderTag(int position)
        {
            Position = position;
        }
        
        public void ShieldTag()
        {
            IsShielded = true;
        }
    }
}
