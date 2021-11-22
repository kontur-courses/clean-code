namespace Markdown
{
    public class ListItemToken : PairedToken
    {
        protected override string MarkdownValue => "- ";

        protected override string HtmlValue => "li";
    }
}
