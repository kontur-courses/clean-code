namespace Markdown
{
    public static class HtmlTag
    {
        public static string CreateStringWithHtmlTag(string str, string tag)
        {
            return $"<{tag}>{str}</{tag}>";
        }
    }
}