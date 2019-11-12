namespace Markdown
{
    public interface IHTMLTag
    {
        string TagName { get; }
        bool IsOpen { get; }
    }
}