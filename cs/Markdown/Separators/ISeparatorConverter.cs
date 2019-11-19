namespace Markdown.Separators
{
    public interface ISeparatorConverter
    {
        string ConvertSeparator(string separator, string value);
    }
}