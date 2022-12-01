namespace Markdown.Interfaces
{
    public interface ITag
    {
        Tag Tag { get; }

        TagType TagType { get; }

        public string ViewTag { get; }
    }
}