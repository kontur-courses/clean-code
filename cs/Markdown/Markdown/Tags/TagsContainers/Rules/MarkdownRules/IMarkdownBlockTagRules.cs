namespace Markdown.Tags.TagsContainers.Rules.MarkdownRules;

public interface IMarkdownBlockTagRules : IMarkdownTagRules
{
    public bool IsBlockOpening(ITag currentTag, ITag nextTag);
    public bool IsBlockClosing(ITag previousTag, ITag currentTag);
}