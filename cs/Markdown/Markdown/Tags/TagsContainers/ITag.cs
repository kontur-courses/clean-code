using Markdown.Tags.TagsContainers.Rules.MarkdownRules;

namespace Markdown.Tags.TagsContainers;

public interface ITag
{
    public TagType Definition { get; }

    public string HtmlOpenTag { get; }

    public string HtmlClosingTag { get; }

    public string MarkdownTag { get; }

    public bool IsMarkdownTagSingle { get; }

    public HashSet<TagType> AllowedNestedTags { get; }

    public IMarkdownTagRules MarkdownRules { get; }
}