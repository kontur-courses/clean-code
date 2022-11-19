namespace Markdown.Tags;

public class HtmlTags
{
    public static Tag Empty => new Tag("", "");
    public static Tag Bold => new Tag("<strong>", "</strong>");
    public static Tag Italics => new Tag("<em>", "</em>");
    public static Tag Heading => new Tag("<h1>", "</h2>");
}