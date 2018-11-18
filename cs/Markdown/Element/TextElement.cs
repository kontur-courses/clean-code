namespace Markdown.Element
{
    public class TextElement : IElement
    {
        public string Indicator { get; set; }
        public string OpenTag { get; set; }
        public string ClosingTag { get; set; }

        public TextElement()
        {
            Indicator = OpenTag = ClosingTag = "";
        }
    }
}
