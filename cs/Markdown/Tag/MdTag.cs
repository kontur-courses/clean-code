namespace Markdown.Tag;

public class MdTag
{
    public MdTag(TagType type, int index)
    { 
        Type = type;
        Index = index;
    }

    public TagType Type { get; }

    public int Index { get; }
}