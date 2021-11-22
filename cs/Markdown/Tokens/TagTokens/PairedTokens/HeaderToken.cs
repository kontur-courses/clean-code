namespace Markdown
{
    public class HeaderToken : PairedToken
    {
        protected override string MarkdownValue => "# ";

        protected override string HtmlValue => "h1";
    }
}
