namespace Markdown.Tag;

public static class TagConfig
{
    public static Dictionary<TagType, string> HtmlTags => new()
    {
        { TagType.Bold, "strong" }, { TagType.Italic, "em" },
        { TagType.Header, "h1" }, { TagType.EscapedSymbol, "" }
    };

    public static Dictionary<TagType, string> MdTags => new()
    {
        { TagType.Bold, "__" }, { TagType.Italic, "_" },
        { TagType.Header, "# " }, { TagType.EscapedSymbol, ""}
    };

    public static Dictionary<TagType, TagType> TagTypes => new()
    {
        {TagType.Bold, TagType.Italic},
        {TagType.Italic, TagType.Bold}
    };
}