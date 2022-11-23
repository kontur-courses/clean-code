namespace Markdown.Tags;

public static class MarkdownTags
{
    public static Tag Bold => _bold ??= new Tag("__", "__");
    public static Tag Italics => _italics ??= new Tag("_", "_");
    public static Tag Heading => _heading ??= new Tag("#", "");
    
    private static Tag? _bold;
    private static Tag? _italics;
    private static Tag? _heading;
}