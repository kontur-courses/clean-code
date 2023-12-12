using Markdown.Tag;

namespace Markdown.TagsMappers;

public class MarkdownToHtmlTagsMapper : ITagsMapper
{
    private readonly Dictionary<string, string> _markdownToHtmlClosingTags = new();
    private readonly Dictionary<string, string> _markdownToHtmlOpenTags = new();

    public string Map(ITag tag)
    {
        throw new NotImplementedException();
    }

    public bool CheckIfMarkdownTag(string tag)
    {
        return _markdownToHtmlClosingTags.ContainsKey(tag) || _markdownToHtmlOpenTags.ContainsKey(tag);
    }

    public int GetMarkdownTagMaxLength()
    {
        throw new NotSupportedException();
    }
}