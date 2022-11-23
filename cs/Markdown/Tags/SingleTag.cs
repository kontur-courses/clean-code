namespace Markdown.Tags;

public class SingleTag
{
    public TagType Type { get; }
    public bool IsClosing { get; }
    public int Index { get; }

    public SingleTag(TagType type, bool isClosing, int index)
    {
        Type = type;
        IsClosing = isClosing;
        Index = index;
    }
}