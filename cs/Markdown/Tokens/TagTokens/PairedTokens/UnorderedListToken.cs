namespace Markdown
{
    public class UnorderedListToken : PairedToken
    {
        protected override string MarkdownValue => "";

        protected override string HtmlValue => "ul";
    }
}

