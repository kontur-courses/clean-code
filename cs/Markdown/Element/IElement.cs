namespace Markdown.Element
{
    public interface IElement
    {
        string Indicator { get; set; }

        string OpenTag { get; set; }

        string ClosingTag { get; set; }
    }
}
