namespace Markdown.Core.Tags.HtmlTags
{
    class H1 : IHtmlTag, IDoubleTag
    {
        public string Opening => "<h1>";
        public string Closing => "</h1>";
    }
}