using Markdown.Primitives;

namespace Markdown;

public static class Tokens
{
    public static Token Text(string text) => new Token(text, TokenType.Text);

    public static Token Italic => new Token($"{Characters.Underline}", TokenType.Italic);

    public static Token Bold => new Token($"{Characters.Underline}{Characters.Underline}", TokenType.Bold);

    public static Token Escape => new Token($"{Characters.Escape}", TokenType.Escape);

    public static Token Header1 => new Token($"{Characters.Sharp}", TokenType.Header1);
    
    public static Token NewLine => new Token($"{Characters.NewLine}", TokenType.NewLine);
}