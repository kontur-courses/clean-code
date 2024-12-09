namespace Markdown.Tags.ConcreteTags;

public class HeaderTag : ITag
{
    public HeaderTag(int position, bool isCloseTag = false)
    {
        Position = position;
        IsCloseTag = isCloseTag;
    }

    public TagType TagType => TagType.Header;

    public int Position { get; set; }

    public bool IsCloseTag { get; set; }
}