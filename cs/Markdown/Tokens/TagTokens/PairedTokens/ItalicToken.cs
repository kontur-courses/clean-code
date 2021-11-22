namespace Markdown
{
    public class ItalicToken : PairedToken
    {
        protected override string MarkdownValue => "_";

        protected override string HtmlValue => "em";
    }
}
