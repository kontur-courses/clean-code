namespace Markdown
{
    public interface ISelectionSymbol
    {
        bool IsClosed
        { get; set; }

        string HtmlTagAnalog
        { get; }

        string SimpleChar
        { get; }

        bool IsStartTag
        { get; set; }
    }
}
