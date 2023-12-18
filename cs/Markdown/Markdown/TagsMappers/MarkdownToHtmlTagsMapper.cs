using System.Collections.Immutable;
using Markdown.Tags;
using Markdown.Tags.TagsContainers;

namespace Markdown.TagsMappers;

public class MarkdownToHtmlTagsMapper : ITagsMapper
{
    private readonly Dictionary<string, string> _markdownToHtmlClosingTags;
    private readonly Dictionary<string, string> _markdownToHtmlOpenTags;

    public MarkdownToHtmlTagsMapper()
    {
        var markdownTags = MarkdownTagsContainer.GetTags();
        var htmlOpenTags = HtmlTagsContainer.GetOpenTags();
        var htmlClosingTags = HtmlTagsContainer.GetClosingTags();

        _markdownToHtmlOpenTags = new Dictionary<string, string>(markdownTags.Count);
        _markdownToHtmlClosingTags = new Dictionary<string, string>(markdownTags.Count);

        BuildTagsConnections(markdownTags, htmlOpenTags, htmlClosingTags);
    }

    public string Map(Tag tag)
    {
        if (CheckIfTag(tag.Value))
        {
            if (tag.TagType == TagType.OpenTag)
                return _markdownToHtmlOpenTags[tag.Value];
            if (tag.TagType == TagType.ClosingTag)
                return _markdownToHtmlClosingTags[tag.Value];
        }

        return tag.Value;
    }

    private bool CheckIfTag(string tag)
    {
        return _markdownToHtmlClosingTags.ContainsKey(tag) || _markdownToHtmlOpenTags.ContainsKey(tag);
    }

    private void BuildTagsConnections(
        ImmutableDictionary<TagDefinition, string> markdownTags,
        ImmutableDictionary<TagDefinition, string> htmlOpenTags,
        ImmutableDictionary<TagDefinition, string> htmlClosingTags)
    {
        foreach (var markdownTag in markdownTags)
        {
            var htmlMatchingOpenTag = htmlOpenTags[markdownTag.Key];
            var htmlMatchingClosingTag = htmlClosingTags[markdownTag.Key];
            _markdownToHtmlOpenTags.Add(markdownTag.Value, htmlMatchingOpenTag);
            _markdownToHtmlClosingTags.Add(markdownTag.Value, htmlMatchingClosingTag);
        }
    }
}