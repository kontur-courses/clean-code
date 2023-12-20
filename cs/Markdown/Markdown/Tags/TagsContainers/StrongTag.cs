using Markdown.Tags.TagsContainers.Rules.MarkdownRules;

namespace Markdown.Tags.TagsContainers;

public class StrongTag : ITag
{
    public TagDefinition Definition => TagDefinition.Strong;

    public string HtmlOpenTag => "<strong>";

    public string HtmlClosingTag => "</strong>";

    public string MarkdownTag => "__";

    public HashSet<TagDefinition> AllowedNestedTags => new() { TagDefinition.Italic };

    public bool IsMarkdownTagSingle => false;

    public IMarkdownTagRules MarkdownRules => new StrongTagRules();
}