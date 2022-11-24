namespace Markdown.Tags.Html
{
    public class HtmlTags
    {
        public static Tag LineBreak => new Tag("<p>", "</p>");
        public static Tag Heading => new Tag("<h1>", "</h1>");

        public static Tag Italics => new Tag("<em>", "</em>");
        public static Tag Bold => new Tag("<strong>", "</strong>");
    }
}