using Markdown.Extensions;
using Markdown.Tags.TextTag;
using Markdown.Tokens;

namespace Markdown.Tags.TagsContainers.Rules.MarkdownRules;

public class UnorderedListComponentTagRules : IMarkdownSingleTagRules
{
    public TagToken GetClosingTag(ITag tag, int paragraphLength)
    {
        var startIndex = paragraphLength;
        var endIndex = startIndex + tag.MarkdownTag.Length;
        var closingToken = new TagToken(startIndex, endIndex, new Tag(tag.MarkdownTag, TagStatus.ClosingTag));
        return closingToken;
    }

    public bool IsTagClosing(char previousSymbol, char nextSymbol)
    {
        return false;
    }

    public bool IsTagIgnoredBySymbol(char symbol, TagStatus tagStatus)
    {
        return false;
    }

    public bool IsTagOpen(char previousSymbol, char nextSymbol)
    {
        return previousSymbol.IsEmpty();
    }

    public bool IsTagsIgnored(TagToken firstTag, TagToken secondTag)
    {
        return false;
    }

    public bool IsTagsPaired(TagToken firstTag, TagToken secondTag, Dictionary<int, TagToken> parsedTokens)
    {
        return false;
    }
}