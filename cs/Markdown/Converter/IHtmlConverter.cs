namespace Markdown.Converter
{
    public interface IHtmlConverter
    {
        string ConvertFromMarkdownToHtml(string markdownText, List<Token> tokens);
    }
}
