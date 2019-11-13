namespace Markdown
{
    public interface ITag
    {
        string Name { get; }
        string Start { get; }
        string End { get; }
        TagType TagType { get; }
    }
}