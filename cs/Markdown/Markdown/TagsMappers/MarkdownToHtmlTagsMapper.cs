using Markdown.Tags.TagsContainers;
using Markdown.Tags.TextTag;

namespace Markdown.TagsMappers;

public class MarkdownToHtmlTagsMapper : ITagsMapper
{
    private static readonly Dictionary<string, ITag> _markdownTags = TagsContainer.GetMarkdownTags();

    public string Map(Tag tag)
    {
        if (CheckIfTag(tag.Value))
        {
            if (tag.TagStatus == TagStatus.OpenTag)
                return _markdownTags[tag.Value].HtmlOpenTag;
            if (tag.TagStatus == TagStatus.ClosingTag)
                return _markdownTags[tag.Value].HtmlClosingTag;
        }

        return tag.Value;
    }

    private static bool CheckIfTag(string tag)
    {
        return _markdownTags.ContainsKey(tag);
    }
}