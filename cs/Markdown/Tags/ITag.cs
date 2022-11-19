
namespace Markdown.Tags
{
    public interface ITag
    {
        TagType Type { get; }
        string Opening { get; }
        string Closing { get; }
    }
}
