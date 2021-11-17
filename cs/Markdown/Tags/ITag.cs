namespace Markdown.Tags
{
    public interface ITag
    {
        TagType Type { get; }
        string Value { get; }
    }
}