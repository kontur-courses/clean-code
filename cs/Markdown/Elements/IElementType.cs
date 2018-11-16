namespace Markdown.Elements
{
    public interface IElementType
    {
        string Indicator { get; }
        bool CanContainElement(IElementType element);
        bool IsIndicatorAt(string markdown, int position);
        bool IsOpeningOfElement(string markdown, int position);
        bool IsClosingOfElement(string markdown, int position);
    }
}
