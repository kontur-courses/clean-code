namespace Markdown.Core.Tags.HtmlTags
{
    internal class Em : IHtmlTag, IDoubleTag
    {
        public string Opening => "<em>";
        public string Closing => "</em>";
    }
}