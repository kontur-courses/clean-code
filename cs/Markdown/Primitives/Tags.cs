namespace Markdown.Primitives;

public static class Tags
{
    public static Tag Text(string value) => new(TagType.Text, value);

    public static Tag Italic(string value) => new(TagType.Italic, value);

    public static Tag Bold(string value) => new(TagType.Bold, value);

    public static Tag Header1(string value) => new(TagType.Header1, value);

    public static Tag Link(string link) => new(TagType.Link, link);
}