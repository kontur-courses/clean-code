namespace Markdown.Tags;

public interface ITag
{
    public TagType TagType { get; }

    public int Position { get; protected set; }

    public bool IsCloseTag { get; protected set; }
}