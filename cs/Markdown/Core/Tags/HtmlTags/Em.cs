namespace Markdown.Core.Tags.HtmlTags
{
    class Em : IHtmlTag, IDoubleTag
    {
        public string Opening => "<em>";
        public string Closing => "</em>";
    }
}