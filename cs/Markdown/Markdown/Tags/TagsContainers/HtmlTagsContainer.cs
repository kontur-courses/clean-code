using System.Collections.Immutable;

namespace Markdown.Tags.TagsContainers;

public static class HtmlTagsContainer
{
    private static readonly ImmutableDictionary<TagDefinition, string> _openTags =
        ImmutableDictionary.CreateRange(
            new[]
            {
                KeyValuePair.Create(TagDefinition.Italic, "\\<em>"),
                KeyValuePair.Create(TagDefinition.Header, "\\<h1>"),
                KeyValuePair.Create(TagDefinition.Strong, "\\<strong>")
            });

    private static readonly ImmutableDictionary<TagDefinition, string> _closingTags =
        ImmutableDictionary.CreateRange(
            new[]
            {
                KeyValuePair.Create(TagDefinition.Italic, "\\</em>"),
                KeyValuePair.Create(TagDefinition.Header, "\\</h1>"),
                KeyValuePair.Create(TagDefinition.Strong, "\\</strong>")
            });

    public static ImmutableDictionary<TagDefinition, string> GetOpenTags()
    {
        return _openTags;
    }

    public static ImmutableDictionary<TagDefinition, string> GetClosingTags()
    {
        return _closingTags;
    }
}