using Markdown.Html.Tags;
using Markdown.Markdown.Attributes;

namespace Markdown.Markdown.Tokens;

public record MarkdownToken(
    string Content,
    TokenType Type,
    TagType TagType = TagType.None,
    IToken? TagPair = null,
    Dictionary<AttributeType, IAttribute>? Attributes = null,
    bool IsCloseTag = false) : IToken;