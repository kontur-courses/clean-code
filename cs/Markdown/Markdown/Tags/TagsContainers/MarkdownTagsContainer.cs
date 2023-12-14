using System.Collections.Immutable;

namespace Markdown.Tags.TagsContainers;

public static class MarkdownTagsContainer
{
    private static readonly ImmutableDictionary<TagDefenition, string> _tags =
        ImmutableDictionary.CreateRange(
            new[]
            {
                KeyValuePair.Create(TagDefenition.Italic, "_"),
                KeyValuePair.Create(TagDefenition.Header, "# "),
                KeyValuePair.Create(TagDefenition.Strong, "__")
            });

    public static ImmutableDictionary<TagDefenition, string> GetTags()
    {
        return _tags;
    }
}