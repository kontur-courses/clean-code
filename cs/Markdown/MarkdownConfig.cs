using Markdown.Tags;

namespace Markdown
{
    public class MarkdownConfig
    {
        public static Dictionary<TagType, string> HtmlTags => new()
        {
        { TagType.Bold, "strong" }, { TagType.Italic, "em" },
        { TagType.Header, "h1" }, { TagType.EscapedSymbol, "" }
        };

        public static Dictionary<TagType, string> MdTags => new()
        {
        { TagType.Bold, "__" }, { TagType.Italic, "_" },
        { TagType.Header, "# " }, { TagType.EscapedSymbol, ""}
        };

        public static Dictionary<TagType, TagType> DifferentTagTypes => new()
        {
        {TagType.Bold, TagType.Italic},
        {TagType.Italic, TagType.Bold}
        };
    }
}
