namespace Markdown.Elements
{
    public interface IElementType
    {
        string Indicator { get; }
        bool CanContainElement(IElementType element);
        bool IsOpeningOfElement(string markdown, bool[] isEscapedCharAt,int position);
        bool IsClosingOfElement(string markdown, bool[] isEscapedCharAt, int position);
    }
}
