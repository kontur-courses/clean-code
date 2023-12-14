﻿using System.Collections.Immutable;

namespace Markdown.Tags.TagsContainers
{
    public static class HtmlTagsContainer
    {
        private static readonly ImmutableDictionary<TagDefenition, string> _openTags =
            ImmutableDictionary.CreateRange(
                new KeyValuePair<TagDefenition, string>[] {
                    KeyValuePair.Create(TagDefenition.Italic, ""),
                    KeyValuePair.Create(TagDefenition.Header, ""),
                    KeyValuePair.Create(TagDefenition.Strong, "")
                    });

        private static readonly ImmutableDictionary<TagDefenition, string> _closingTags =
            ImmutableDictionary.CreateRange(
                new KeyValuePair<TagDefenition, string>[] {
                    KeyValuePair.Create(TagDefenition.Italic, ""),
                    KeyValuePair.Create(TagDefenition.Header, ""),
                    KeyValuePair.Create(TagDefenition.Strong, "")
                    });

        public static ImmutableDictionary<TagDefenition, string> GetOpenTags() => _openTags;
        public static ImmutableDictionary<TagDefenition, string> GetClosingTags() => _closingTags;
    }
}
