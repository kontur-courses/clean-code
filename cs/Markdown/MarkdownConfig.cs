using Markdown.Tags;

namespace Markdown
{
    public class MarkdownConfig
    {
        public static Dictionary<Tag, string> HtmlTags => new()
        {
        { Tag.Bold, "strong" }, { Tag.Italic, "em" },
        { Tag.Header, "h1" }, { Tag.EscapedSymbol, "" }
        };

        public static Dictionary<Tag, string> MdTags => new()
        {
        { Tag.Bold, "__" }, { Tag.Italic, "_" },
        { Tag.Header, "# " }, { Tag.EscapedSymbol, ""}
        };

        public static Dictionary<Tag, Tag> DifferentTags => new()
        {
        {Tag.Bold, Tag.Italic},
        {Tag.Italic, Tag.Bold}
        };
    }
}
