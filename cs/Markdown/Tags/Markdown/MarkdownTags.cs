namespace Markdown.Tags.Markdown
{
    public class MarkdownTags
    {
        public static Tag Heading => new Tag("#");

        public static Tag Italics => new Tag("_")
        {
            IgnoredTags = new[] { Bold }
        };

        public static Tag Bold => new Tag("__");
        
    }
}