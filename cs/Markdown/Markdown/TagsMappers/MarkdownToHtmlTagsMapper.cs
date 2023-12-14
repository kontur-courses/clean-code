using Markdown.Tags;

namespace Markdown.TagsMappers;

public class MarkdownToHtmlTagsMapper : ITagsMapper
{
    private readonly Dictionary<string, string> _markdownToHtmlClosingTags;
    private readonly Dictionary<string, string> _markdownToHtmlOpenTags;

    public MarkdownToHtmlTagsMapper()
    {
        throw new NotImplementedException();
    }

    public string Map(Tag tag)
    {
        throw new NotImplementedException();
    }

    private bool CheckIfTag(string tag)
    {
        throw new NotImplementedException();
    }
}