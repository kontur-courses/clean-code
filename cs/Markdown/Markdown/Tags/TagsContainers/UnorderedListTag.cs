using Markdown.Tags.TagsContainers.Rules.MarkdownRules;

namespace Markdown.Tags.TagsContainers;

public class UnorderedListTag : ITag
{
    public TagType Type => TagType.UnorderedList;

    public string HtmlOpenTag => "<ul>";

    public string HtmlClosingTag => "</ul>";

    public string MarkdownTag => "* ";

    public bool IsMarkdownTagSingle => false;

    public HashSet<TagType> AllowedNestedTags => new() { TagType.ListComponent };

    public IMarkdownTagRules MarkdownRules => new UnorderedListTagRules();

    public bool IsTagComponent => false;
}