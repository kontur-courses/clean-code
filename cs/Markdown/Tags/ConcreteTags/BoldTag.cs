namespace Markdown.Tags.ConcreteTags;

public class BoldTag : ITag
{
    public BoldTag(int position, bool isCloseTag = false)
    {
        Position = position;
        IsCloseTag = isCloseTag;
    }

    public TagType TagType => TagType.Bold;

    public int Position { get; set; }

    public bool IsCloseTag { get; set; }
}