namespace Markdown.Html
{
    public interface IHtmlConverter
    {
        string ConvertSeparatedStringToPairedHtmlTag(string value, string separator);
    }
}