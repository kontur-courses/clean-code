namespace Markdown.Shell
{
    public interface IShell
    {
        bool Contains(IShell shell);
        bool TryOpen(string text, int startPrefix, out MatchObject matchObject);
        bool TryClose(string text, int startSuffix, out MatchObject matchObject);
    }
}
