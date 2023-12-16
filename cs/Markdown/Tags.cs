namespace Markdown;

public static class Tags
{
    public static Dictionary<string, Tag> TagDict = new()
    {
        { "__", new Tag(true, TagType.Bold, new[] { "_" }) },
        { "_", new Tag(true, TagType.Italic) },
        { "# ", new Tag(false, TagType.Heading) },
        { "\n", new Tag(false, TagType.LineBreaker) },
        { "\r\n", new Tag(false, TagType.LineBreaker) }
    };

    public static readonly Dictionary<string, string> HtmlTagDict = new()
    {
        { "_", "<em>" },
        { "__", "<strong>" },
        { "# ", "<h1>" },
        { "\n", "</h1>" },
        { "\r\n", "</h1>" },
        { @"\", "" }
    };
}