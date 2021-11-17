namespace Markdown
{
    public interface ISelectionSymbol
    {
        bool IsClosed
        { get; set; }

        string HtmlTagAnalog
        { get; }
    }
}
