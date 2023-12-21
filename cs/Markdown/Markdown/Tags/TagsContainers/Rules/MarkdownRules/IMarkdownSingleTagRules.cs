using Markdown.Tokens;

namespace Markdown.Tags.TagsContainers.Rules.MarkdownRules;

public interface IMarkdownSingleTagRules : IMarkdownTagRules
{
    public TagToken GetClosingTag(ITag tag, int paragraphLength);
}