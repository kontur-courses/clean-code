namespace Markdown.Core.Tags.HtmlTags
{
    internal class Strong : IHtmlTag, IDoubleTag
    {
        public string Opening => "<strong>";
        public string Closing => "</strong>";
    }
}