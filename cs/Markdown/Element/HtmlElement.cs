namespace Markdown.Element
{
    public class HtmlElement : IElement
    {
        public string Indicator { get; set; }
        public string OpenTag { get; set; }
        public string ClosingTag { get; set; }

        public HtmlElement(string indicator, string htmlTag)
        {
            Indicator = indicator;
            OpenTag = htmlTag;
            ClosingTag = GenerateClosingTag(htmlTag);
        }

        private static string GenerateClosingTag(string htmlTag)
        {
            return htmlTag.Insert(1, "/");
        }
    }
}
