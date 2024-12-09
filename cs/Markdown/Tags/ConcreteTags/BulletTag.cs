namespace Markdown.Tags.ConcreteTags;

public class BulletTag : ITag
{
    public BulletTag(int position, bool isCloseTag = false)
    {
        Position = position;
        IsCloseTag = isCloseTag;
    }

    public TagType TagType => TagType.BulletedListItem;
    public int Position { get; set; }
    public bool IsCloseTag { get; set; }
}