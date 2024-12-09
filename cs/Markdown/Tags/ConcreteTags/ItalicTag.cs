namespace Markdown.Tags.ConcreteTags;

public class ItalicTag : ITag
{
    public ItalicTag(int position, bool isCloseTag = false)
    {
        Position = position;
        IsCloseTag = isCloseTag;
    }

    public TagType TagType => TagType.Italic;

    public int Position { get; set; }

    public bool IsCloseTag { get; set; }
}