namespace Markdown.SeparatorConverters
{
    public interface ISeparatorConverter
    {
        string ConvertSeparator(string separator, string value);
    }
}