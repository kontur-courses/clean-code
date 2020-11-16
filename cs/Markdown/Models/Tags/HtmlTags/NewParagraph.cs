namespace Markdown.Models.Tags.HtmlTags
{
    internal class NewParagraph : Tag
    {
        public override string Opening => "<p>";
        public override string Closing => "</p>";
    }
}
