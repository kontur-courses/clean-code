using Markdown.Tags.TextTag;

namespace Markdown.TagsMappers;

public interface ITagsMapper
{
    string Map(Tag tag);
}