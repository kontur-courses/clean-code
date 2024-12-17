using Markdown.Html.Tags;
using Markdown.Markdown.Attributes;

namespace Markdown.Markdown.Tokens;

public interface IToken
{
    TokenType Type { get; }
    string Content { get; }
    bool IsCloseTag { get; }
    TagType TagType { get; }
    IToken? TagPair { get; }
    Dictionary<AttributeType, IAttribute>? Attributes { get; }
}