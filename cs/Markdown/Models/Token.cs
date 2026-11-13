namespace Markdown.Models;

public class Token(TokenKind kind, string text = "")
{
    public TokenKind Kind { get; } = kind;
    public string Text { get; } = text;
}