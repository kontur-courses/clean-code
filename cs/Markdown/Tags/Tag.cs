namespace Markdown.Tags;

public class Tag
{
    public TagType Type { get; }
    public TagPosition Position { get; }

    public Tag(TagPosition tag, TagType type)
    {
        Position = tag;
        Type = type;
    }
}