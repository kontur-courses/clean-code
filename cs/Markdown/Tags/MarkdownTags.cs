namespace Markdown.Tags;

public static class MarkdownTags
{
    public static Tag Empty => new Tag("", "");
    public static Tag Bold => new Tag("__", "__");
    public static Tag Italics => new Tag("_", "_");
    public static Tag Heading => new Tag("#", "");
}