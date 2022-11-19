namespace Markdown;

public class MarkdownToHtmlConverter
{
    public static readonly Dictionary<string, TokenType> MarkdownTags = new()
    {
        // Order matters
        [@"\"] = TokenType.Escape,
        ["# "] = TokenType.Header,
        ["__"] = TokenType.Bold,
        ["_"] = TokenType.Italic,
        [" "] = TokenType.Space
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

    public static List<Token> Convert(List<Token> tokens)
    {
        foreach (var t in tokens.Where(t =>
                     t.TokensType is not (TokenType.Text or TokenType.Space or TokenType.Escape)))
            t.Text = HtmlTags[(t.TokensType, t.IsOpening)];

        return tokens;
    }
}