using Markdown.Tags.TagsContainers.Rules.MarkdownRules;

namespace Markdown.Tags.TagsContainers;

public class StrongTag : ITag
{
    public TagType Type => TagType.Strong;

    public string HtmlOpenTag => "<strong>";

    public string HtmlClosingTag => "</strong>";

    public string MarkdownTag => "__";

    public HashSet<TagType> AllowedNestedTags => new() { TagType.Italic };

    public bool IsMarkdownTagSingle => false;

    public IMarkdownTagRules MarkdownRules => new StrongTagRules();
}