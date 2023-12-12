using Markdown.Tag;

namespace Markdown.TagsMappers;

public interface ITagsMapper
{
    string Map(ITag tag);
}