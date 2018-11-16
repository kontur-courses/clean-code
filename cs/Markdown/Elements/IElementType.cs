namespace Markdown.Elements
{
    public interface IElementType
    {
        string Indicator { get; }
        bool CanContainElement(IElementType element);
        bool IsOpeningOfElement(string markdown, bool[] escapeBitMask,int position);
        bool IsClosingOfElement(string markdown, bool[] escapeBitMask, int position);
    }
}
