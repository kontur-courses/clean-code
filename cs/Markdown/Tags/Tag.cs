namespace Markdown.Tags;

public class Tag
{
    public TagType Type { get; }
    public TagPosition Position { get; }

    public Tag(TagPosition position, TagType type)
    {
        Position = position;
        Type = type;
    }

    public (SingleTag,SingleTag) ConvertToPairOfSingleTags()
    {
        throw new NotImplementedException();
    }
}