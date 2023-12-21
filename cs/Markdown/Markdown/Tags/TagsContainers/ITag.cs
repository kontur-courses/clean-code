using Markdown.Tags.TagsContainers.Rules.MarkdownRules;

namespace Markdown.Tags.TagsContainers;

public interface ITag
{
    public TagType Type { get; }

    public string HtmlOpenTag { get; }

    public string HtmlClosingTag { get; }

    public string MarkdownTag { get; }

    public bool IsMarkdownTagSingle { get; }
    public bool IsTagComponent { get; }

    public HashSet<TagType> AllowedNestedTags { get; }

    public IMarkdownTagRules MarkdownRules { get; }
}