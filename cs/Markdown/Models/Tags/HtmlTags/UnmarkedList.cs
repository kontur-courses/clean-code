namespace Markdown.Models.Tags.HtmlTags
{
    internal class UnmarkedList : Tag
    {
        public override string Opening => "<ul>";
        public override string Closing => "</ul>";
    }
}
