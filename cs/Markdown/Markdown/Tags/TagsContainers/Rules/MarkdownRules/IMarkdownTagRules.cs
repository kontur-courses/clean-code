using Markdown.Tags.TextTag;
using Markdown.Tokens;

namespace Markdown.Tags.TagsContainers.Rules.MarkdownRules;

public interface IMarkdownTagRules
{
    public bool IsTagIgnoredBySymbol(char symbol, TagStatus tagStatus);

    public bool IsTagOpen(char previousSymbol, char nextSymbol);

    public bool IsTagClosing(char previousSymbol, char nextSymbol);

    public bool IsTagsPaired(TagToken firstTag, TagToken secondTag, Dictionary<int, TagToken> parsedTokens);

    public bool IsTagsIgnored(TagToken firstTag, TagToken secondTag);
}