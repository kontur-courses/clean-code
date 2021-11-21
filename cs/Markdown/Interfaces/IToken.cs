namespace Markdown.Interfaces
{
    public interface IToken
    {
        bool IsOpenTag { get; set; }

        string ConvertToHtml();
    }
}