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
        throw new NotImplementedException();
    }

    private bool CheckIfTag(string tag)
    {
        throw new NotImplementedException();
    }

    public void BuildTagsConnections(
        ImmutableDictionary<TagDefenition, string> markdownTags,
        ImmutableDictionary<TagDefenition, string> htmlOpenTags,
        ImmutableDictionary<TagDefenition, string> htmlClosingTags)
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