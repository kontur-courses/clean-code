namespace Markdown
{
    public interface IMarkupProcessor
    {
        string GetHtmlMarkup(string text);
    }
}