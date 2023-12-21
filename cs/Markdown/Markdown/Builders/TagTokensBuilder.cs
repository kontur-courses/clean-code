using System.Text;
using Markdown.Tags.TextTag;
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
        var result = new StringBuilder();
        foreach (var token in tokens)
        {
            var mappedToken = _tagsMapper.Map(token.Value);
            result.Append(mappedToken);
        }

        return result.ToString();
    }
}