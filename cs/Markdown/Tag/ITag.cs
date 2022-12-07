namespace Markdown
{
    public interface ITag
    {
        string Opening { get; }
        string Closing { get; }
        bool IsSelfClosing { get; }
        bool CanNesting { get; }
    }
}
