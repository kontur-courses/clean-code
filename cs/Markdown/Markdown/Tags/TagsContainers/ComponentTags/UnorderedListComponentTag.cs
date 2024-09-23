using Markdown.Tags.TagsContainers.Rules.MarkdownRules;

namespace Markdown.Tags.TagsContainers.ComponentTags;

public class UnorderedListComponentTag : IComponentTag
{
    public TagType Type => TagType.ListComponent;

    public string HtmlOpenTag => "<li>";

    public string HtmlClosingTag => "</li>";

    public string MarkdownTag => "* ";

    public bool IsMarkdownTagSingle => true;

    public HashSet<TagType> AllowedNestedTags => new() { TagType.Italic, TagType.Strong };

    public IMarkdownTagRules MarkdownRules => new UnorderedListComponentTagRules();

    public ITag BlockTag => new UnorderedListTag();

    public bool IsTagComponent => true;
}