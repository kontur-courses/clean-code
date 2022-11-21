using System.Text;

namespace Markdown;

public class MarkdownToHtmlConverter
{
    public static readonly Dictionary<string, TokenType> MarkdownTags = new()
    {
        // Order matters
        ["# "] = TokenType.Header,
        ["__"] = TokenType.Bold,
        ["_"] = TokenType.Italic,
    };

    private static readonly Dictionary<(TokenType type, bool isOpening), string> HtmlTags = new()
    {
        [(TokenType.Header, true)] = "<h1>",
        [(TokenType.Header, false)] = "</h1>",
        [(TokenType.Bold, true)] = "<strong>",
        [(TokenType.Bold, false)] = "</strong>",
        [(TokenType.Italic, true)] = "<em>",
        [(TokenType.Italic, false)] = "</em>"
    };

    public static List<TokenBase> ToHtml(StringBuilder text, List<TokenBase> tokens)
    {
        return tokens;
    }
}