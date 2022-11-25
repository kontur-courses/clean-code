namespace Markdown.DataStructures
{
    public interface ITag
    {
        string OpeningTag { get; }
        string ClosingTag { get; }
        string MarkdownName { get; }
        string TagContent { get; }
    }
}