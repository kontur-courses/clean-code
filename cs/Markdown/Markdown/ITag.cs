namespace Markdown
{
    public interface ITag
    {
        TagType Type { get; }
        int Start { get; }
        int End { get; }
    }
}