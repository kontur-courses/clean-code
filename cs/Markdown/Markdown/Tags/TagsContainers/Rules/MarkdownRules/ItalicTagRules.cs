using Markdown.Extensions;
using Markdown.Tags.TextTag;
using Markdown.Tokens;
using static Markdown.Tags.TagsContainers.Rules.Utils;

namespace Markdown.Tags.TagsContainers.Rules.MarkdownRules;

public class ItalicTagRules : IMarkdownTagRules
{
    public bool IsTagIgnoredBySymbol(char symbol, TagStatus tagType)
    {
        return char.IsDigit(symbol) && tagType == TagStatus.Undefined;
    }

    public bool IsTagOpen(char previousSymbol, char nextSymbol)
    {
        return (char.IsWhiteSpace(previousSymbol) && !char.IsWhiteSpace(nextSymbol)) || previousSymbol.IsEmpty();
    }

    public bool IsTagClosing(char previousSymbol, char nextSymbol)
    {
        return (char.IsWhiteSpace(nextSymbol) && !char.IsWhiteSpace(previousSymbol)) || nextSymbol.IsEmpty();
    }

    public bool IsTagsPaired(TagToken firstTag, TagToken secondTag, Dictionary<int, TagToken> parsedTokens)
    {
        if ((firstTag.Value.TagType == TagStatus.Undefined || secondTag.Value.TagType == TagStatus.Undefined)
            && IsTagsInsideOneWord(firstTag, secondTag, parsedTokens))
            return true;

        return firstTag.Value.TagType == TagStatus.OpenTag && secondTag.Value.TagType == TagStatus.ClosingTag;
    }

    public bool IsTagsIgnored(TagToken firstTag, TagToken secondTag)
    {
        if (firstTag.ToString() != secondTag.ToString())
            return false;

        return firstTag.EndIndex + 1 == secondTag.StartIndex;
    }
}