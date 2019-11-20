namespace Markdown.Core.Tags.HtmlTags
{
    class Q : IHtmlTag, IDoubleTag
    {
        public string Opening => "<q>";
        public string Closing => "</q>";
    }
}