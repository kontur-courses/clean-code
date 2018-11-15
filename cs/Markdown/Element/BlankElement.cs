namespace Markdown.Element
{
    public class BlankElement : IElement
    {
        public string Indicator { get; set; }
        public string OpenTag { get; set; }
        public string ClosingTag { get; set; }

        public BlankElement()
        {
            Indicator = OpenTag = ClosingTag = "";
        }
    }
}
