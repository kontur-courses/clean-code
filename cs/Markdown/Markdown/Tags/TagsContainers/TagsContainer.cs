using System.Collections.Immutable;
using Markdown.Tags.TagsContainers.ComponentTags;

namespace Markdown.Tags.TagsContainers;

public static class TagsContainer
{
    private static readonly ImmutableArray<ITag> _tags = ImmutableArray.Create(
        new ITag[]
        {
            new ItalicTag(),
            new StrongTag(),
            new HeaderTag(),
            new UnorderedListComponentTag(),
            new UnorderedListTag(),
        });

    public static List<ITag> GetTags()
    {
        return _tags.ToList();
    }

    public static Dictionary<string, ITag> GetMarkdownTags()
    {
        var markdonwTags = _tags.ToDictionary(tag => tag.MarkdownTag, tag => tag);

        return markdonwTags;
    }
}