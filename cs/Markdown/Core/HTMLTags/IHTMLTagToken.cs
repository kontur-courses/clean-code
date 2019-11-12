namespace Markdown
{
    public interface IHTMLTagToken
    {
        string TagName { get; }
        bool IsOpen { get; }
    }
}