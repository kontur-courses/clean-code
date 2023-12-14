using System.Collections.Immutable;

namespace Markdown.Tags.TagsContainers
{
    public static class MarkdownTagsContainer
    {
        private static readonly ImmutableDictionary<TagDefenition, string> _openTags =
            ImmutableDictionary.CreateRange(
                new KeyValuePair<TagDefenition, string>[] {
                    KeyValuePair.Create(TagDefenition.Italic, "_"),
                    KeyValuePair.Create(TagDefenition.Header, "# "),
                    KeyValuePair.Create(TagDefenition.Strong, "__")
                    });

        private static readonly ImmutableDictionary<TagDefenition, string> _closingTags =
            ImmutableDictionary.CreateRange(
                new KeyValuePair<TagDefenition, string>[] {
                    KeyValuePair.Create(TagDefenition.Italic, "_"),
                    KeyValuePair.Create(TagDefenition.Header, "# "),
                    KeyValuePair.Create(TagDefenition.Strong, "__")
                    });

        public static ImmutableDictionary<TagDefenition, string> GetOpenTags() => _openTags;
        public static ImmutableDictionary<TagDefenition, string> GetClosingTags() => _closingTags;
    }
}
