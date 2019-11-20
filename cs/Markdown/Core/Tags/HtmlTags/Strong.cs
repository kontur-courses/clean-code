namespace Markdown.Core.Tags.HtmlTags
{
    class Strong : IHtmlTag, IDoubleTag
    {
        public string Opening => "<strong>";
        public string Closing => "</strong>";
    }
}