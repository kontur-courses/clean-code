using Markdown.Tags;

namespace Markdown.Converter
{
    public static class MarkupConverter
    {
        public static string ConvertTagToHtmlString(Tag tag, string value)
        {
            return tag != null 
                ? $"<{tag.HtmlValue}>{value}</{tag.HtmlValue}>"
                : value;
        }
    }
}