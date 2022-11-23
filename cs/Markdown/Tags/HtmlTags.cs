namespace Markdown.Tags;

public class HtmlTags
{
    public static Tag Bold => _bold ??= new Tag("<strong>", "</strong>");
    public static Tag Italics => _italics ??= new Tag("<em>", "</em>");
    public static Tag Heading => _heading ??= new Tag("<h1>", "</h1>");

    private static Tag? _bold;
    private static Tag? _italics;
    private static Tag? _heading;
}