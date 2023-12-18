using System.Collections.Immutable;

namespace Markdown.Tags.TagsContainers;

public static class MarkdownTagsContainer
{
    private static readonly ImmutableDictionary<TagDefinition, string> _tags =
        ImmutableDictionary.CreateRange(
            new[]
            {
                KeyValuePair.Create(TagDefinition.Italic, "_"),
                KeyValuePair.Create(TagDefinition.Header, "# "),
                KeyValuePair.Create(TagDefinition.Strong, "__")
            });

    public static ImmutableDictionary<TagDefinition, string> GetTags()
    {
        return _tags;
    }
}