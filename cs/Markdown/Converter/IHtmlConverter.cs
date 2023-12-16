namespace Markdown.Converter
{
    public interface IHtmlConverter
    {
        string ConvertFromMarkdownToHtml(List<Token> tokens);
    }
}
