using Markdown.Tags.TagsContainers.Rules.MarkdownRules;

namespace Markdown.Tags.TagsContainers;

public interface ITag
{
    public TagDefinition Definition { get; }

    public string HtmlOpenTag { get; }

    public string HtmlClosingTag { get; }

    public string MarkdownTag { get; }

    public bool IsMarkdownTagSingle { get; }

    public HashSet<TagDefinition> AllowedNestedTags { get; }

    public IMarkdownTagRules MarkdownRules { get; }
}