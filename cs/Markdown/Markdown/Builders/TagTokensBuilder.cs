using Markdown.Tags;
using Markdown.TagsMappers;
using Markdown.Tokens;

namespace Markdown.Builders;

public class TagTokensBuilder : ITokensBuilder<Tag>
{
    private readonly ITagsMapper _tagsMapper;

    public TagTokensBuilder(ITagsMapper mapper)
    {
        _tagsMapper = mapper;
    }

    public string Build(List<IToken<Tag>> tokens)
    {
        throw new NotImplementedException();
    }
}