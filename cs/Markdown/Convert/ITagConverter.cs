using Markdown.Tags;

namespace Markdown.Convert;

public interface ITagConverter
{
    public Tag Convert(Tag tag);
}