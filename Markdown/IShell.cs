namespace Markdown
{
    public interface IShell
    {
        string GetPrefix();
        string GetSuffix();
        string RenderToHtml(string text);
        bool Contains(IShell shell);
    }
}
