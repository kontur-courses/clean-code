namespace Markdown.Models.Tags.HtmlTags
{
    internal class UnorderedListElement : Tag
    {
        public override string Opening => "<li>";
        public override string Closing => "</li>";
    }
}
