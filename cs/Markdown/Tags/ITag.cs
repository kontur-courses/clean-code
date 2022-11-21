namespace Markdown.Tags
{
    public interface ITag
    {
        public string Opening { get; }
        public string Closing { get; }
    }
}