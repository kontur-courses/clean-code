using Markdown.Tags.TagsContainers.Rules.MarkdownRules;

namespace Markdown.Tags.TagsContainers;

public class ItalicTag : ITag
{
    public TagType Type => TagType.Italic;

    public string HtmlOpenTag => "<em>";

    public string HtmlClosingTag => "</em>";

    public string MarkdownTag => "_";

    public HashSet<TagType> AllowedNestedTags => new();

    public bool IsMarkdownTagSingle => false;

    public IMarkdownTagRules MarkdownRules => new ItalicTagRules();
}