namespace Markdown.Tags.Markdown
{
    public class MarkdownTags
    {
        public static Tag Heading => new Tag("#");

        public static Tag Italics => new Tag("_", "_")
        {
            IgnoredTags = new[] { Bold }
        };

        public static Tag Bold => new Tag("__", "__");

        public static Tag Link => new Tag("[]()");

    }
}