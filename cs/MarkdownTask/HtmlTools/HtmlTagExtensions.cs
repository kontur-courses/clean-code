using static MarkdownTask.TagInfo;

namespace MarkdownTask.HtmlTools
{
    public static class HtmlTagExtensions
    {
        public static string GetHtmlString(this TagType tag) => tag switch
        {
            TagType.Header => "<h1>",
            TagType.Italic => "<em>",
            TagType.Strong => "<strong>",
            _ => ""
        };
    }
}
