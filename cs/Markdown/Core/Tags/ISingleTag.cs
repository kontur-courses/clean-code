namespace Markdown.Core.Tags
{
    public interface ISingleTag : ITag
    {
        string Opening { get; }
    }
}