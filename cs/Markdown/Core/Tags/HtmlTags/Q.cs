namespace Markdown.Core.Tags.HtmlTags
{
    internal class Q : IHtmlTag, IDoubleTag
    {
        public string Opening => "<q>";
        public string Closing => "</q>";
    }
}