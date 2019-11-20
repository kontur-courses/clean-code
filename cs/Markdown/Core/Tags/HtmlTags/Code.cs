namespace Markdown.Core.Tags.HtmlTags
{
    class Code : IHtmlTag, IDoubleTag
    {
        public string Opening => "<code>";
        public string Closing => "</code>";
    }
}