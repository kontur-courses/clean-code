using Markdown.Tags.TextTag;
using Markdown.Tokens;

namespace Markdown.Tags.TagsContainers.Rules.MarkdownRules;

public class UnorderedListTagRules : IMarkdownBlockTagRules
{
    public bool IsBlockClosing(ITag currentTag, ITag nextTag)
    {
        if (currentTag == null) return false;
        return currentTag.Type == TagType.ListComponent && (nextTag == null || nextTag.Type != TagType.ListComponent);
    }

    public bool IsBlockOpening(ITag previousTag, ITag currentTag)
    {
        if (currentTag == null) return false;
        return currentTag.Type == TagType.ListComponent &&
               (previousTag == null || previousTag.Type != TagType.ListComponent);
    }

    public bool IsTagClosing(char previousSymbol, char nextSymbol)
    {
        return false;
    }

    public bool IsTagIgnoredBySymbol(char symbol, TagStatus tagType)
    {
        return false;
    }

    public bool IsTagOpen(char previousSymbol, char nextSymbol)
    {
        return false;
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