namespace Markdown.Models.Tags.HtmlTags
{
    internal class H1 : Tag
    {
        public override string Opening => "<h1>";
        public override string Closing => "</h1>";
    }
}
