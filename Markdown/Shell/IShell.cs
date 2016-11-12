namespace Markdown.Shell
{
    public interface IShell
    {
        string GetPrefix();
        string GetSuffix();
        string RenderToHtml(string text);
        bool Contains(IShell shell);
        bool IsRestricted(string text, int startSuffix);
    }
}
