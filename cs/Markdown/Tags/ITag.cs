namespace Markdown.Tags
{
    public interface ITag
    {
        public string Opening { get; set; }
        public string Closing { get; set; }
        public TagType Type { get; set; }
    }
}