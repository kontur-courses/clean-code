namespace Markdown
{
    public class BoldToken : PairedToken
    {
        protected override string MarkdownValue => "__";

        protected override string HtmlValue => "strong";
    }
}
