using Markdown.Nodes.HtmlTags;

namespace Markdown.Parsing;

public static class TokenTypeExtensions
{
    public static HtmlTag ToHtmlTag(this TokenType type)
    {
        return type switch
        {
            TokenType.Italic => HtmlTag.Em,
            TokenType.Bold => HtmlTag.Strong,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Unknown token type")
        };
    }
}