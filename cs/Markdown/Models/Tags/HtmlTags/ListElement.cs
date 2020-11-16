namespace Markdown.Models.Tags.HtmlTags
{
    internal class ListElement : Tag
    {
        public override string Opening => "<li>";
        public override string Closing => "</li>";
    }
}
