namespace Markdown.Core.Tags.HtmlTags
{
    internal class Code : IHtmlTag, IDoubleTag
    {
        public string Opening => "<code>";
        public string Closing => "</code>";
    }
}