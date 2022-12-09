namespace Markdown.Tag
{
    public interface ITag
    {
        string Opening { get; }
        string Closing { get; }
        bool IsSelfClosing { get; }
        bool CanNesting { get; }
    }
}
