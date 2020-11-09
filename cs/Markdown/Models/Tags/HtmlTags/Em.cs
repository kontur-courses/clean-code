namespace Markdown.Models.Tags.HtmlTags
{
    internal class Em : Tag
    {
        public override string Opening => "<em>";
        public override string Closing => "</em>";
    }
}
