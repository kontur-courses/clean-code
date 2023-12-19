namespace Markdown.Tags
{
    public interface ITag
    {
        public TagType Type { get; }
        public int Position { get; }
        public bool IsEndTag { get; }
        public bool IsShielded { get; }
        public void ShieldTag();
    }
}
