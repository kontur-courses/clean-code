namespace Markdown;

public class Tag
{
    public readonly string[] BlockTags;
    public bool IsOpen;
    public bool IsPaired;
    public TagType Type;

    public Tag(bool isPaired, TagType type, string[]? blockTags = null)
    {
        BlockTags = blockTags ?? Array.Empty<string>();
        Type = type;
        IsPaired = isPaired;
    }
}