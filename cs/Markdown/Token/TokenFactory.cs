namespace Markdown;

public static class TokenFactory
{
    public static Token Create(TokenType type, string? info)
        => new Token { Type = type, Value = info };
}