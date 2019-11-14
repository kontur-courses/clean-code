namespace Markdown.Html
{
    public class HtmlTag
    {
        public static HtmlTag Italic = new HtmlTag("em");
        public static HtmlTag Bold = new HtmlTag("strong");


        private readonly string name;

        public HtmlTag(string name)
        {
            this.name = name;
        }

        public string ApplyTo(string value)
        {
            return $"<{name}>{value}</{name}>";
        }
    }
}