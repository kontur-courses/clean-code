using Markdown.TagsMappers;

namespace Markdown.Builders;

public class MarkdownToHtmlBuilder
{
    private readonly ITagsMapper _tagsMapper;

    public MarkdownToHtmlBuilder()
    {
        _tagsMapper = new MarkdownToHtmlTagsMapper();
    }

    public string Build(List<Tag.Tag> tokens)
    {
        throw new NotImplementedException();
    }
}