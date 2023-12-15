namespace Markdown;



public class Tag
{
    public bool IsPaired = false;
    public TagType Type;
    public readonly string[] BlockTags;
    public bool IsOpen;
    public Tag(bool isPaired, TagType type, string[]? blockTags = null)
    {
        this.BlockTags = blockTags ?? Array.Empty<string>();
        Type = type;
        this.IsPaired = isPaired;
    }
}