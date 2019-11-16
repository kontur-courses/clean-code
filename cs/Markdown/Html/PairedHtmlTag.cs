namespace Markdown.Html
{
    public class PairedHtmlTag : HtmlTag
    {
        public static PairedHtmlTag Italic = new PairedHtmlTag("em");
        public static PairedHtmlTag Bold = new PairedHtmlTag("strong");


        public PairedHtmlTag(string name) : base(name) { }

        public string ApplyTo(string value)
        {
            return $"<{Name}>{value}</{Name}>";
        }
    }
}