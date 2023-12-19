using Markdown.Tags.TagsContainers.Rules.MarkdownRules;

namespace Markdown.Tags.TagsContainers;

public class HeaderTag : ITag
{
    public TagDefinition Definition => TagDefinition.Header;

    public string HtmlOpenTag => "\\<h1>";

    public string HtmlClosingTag => "\\</h1>";

    public string MarkdownTag => "# ";

    public HashSet<TagDefinition> AllowedNestedTags => new() { TagDefinition.Italic, TagDefinition.Strong };

    public bool IsMarkdownTagSingle => true;

    public IMarkdownTagRules MarkdownRules => new HeaderTagRules();
}