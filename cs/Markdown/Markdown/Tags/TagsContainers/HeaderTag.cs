using Markdown.Tags.TagsContainers.Rules.MarkdownRules;

namespace Markdown.Tags.TagsContainers;

public class HeaderTag : ITag
{
    public TagType Definition => TagType.Header;

    public string HtmlOpenTag => "<h1>";

    public string HtmlClosingTag => "</h1>";

    public string MarkdownTag => "# ";

    public HashSet<TagType> AllowedNestedTags => new() { TagType.Italic, TagType.Strong };

    public bool IsMarkdownTagSingle => true;

    public IMarkdownTagRules MarkdownRules => new HeaderTagRules();
}