namespace Markdown
{
    public interface ITag
    {
        string Name { get; }
        string GetClosing();
        string GetOpener();
        string EncloseInTags(string line);
    }
}