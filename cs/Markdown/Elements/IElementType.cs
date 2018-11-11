namespace Markdown.Elements
{
    public interface IElementType
    {
        string Indicator { get; }
        bool CanContainElement(IElementType element);
    }
}
