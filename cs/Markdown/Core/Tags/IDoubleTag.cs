namespace Markdown.Core.Tags
{
    public interface IDoubleTag : ITag
    {
        string Opening { get; }
        string Closing { get; }
    }
}