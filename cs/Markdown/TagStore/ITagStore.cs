using Markdown.Tags;

namespace Markdown.TagStore
{
    public interface ITagStore
    {
        string ToString(Tags.TagType type);
        Tags.TagType ToTag(string value);
        string[] GetTagsValues();
    }
}