namespace Markdown.Tags
{
    public interface ITag
    {
        public abstract TagType Type { get; }
        public abstract string StartTag { get; }
        public abstract string? EndTag { get; }
        public int Position { get; }
    }
}
