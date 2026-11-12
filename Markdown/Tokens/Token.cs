using Markdown.Tokens.Lexing;

namespace Markdown.Tokens;

public sealed record TokenType(string Name)
{
    public static readonly TokenType Text = new("text");
    public static readonly TokenType Underscore = new("underscore");
    public static readonly TokenType DoubleUnderscore = new("double_underscore");
    public static readonly TokenType HeaderMarker = new("header_marker");
    public static readonly TokenType ParagraphBreak = new("paragraph_break");
}

public abstract record Token(TokenType Type, string Value);

public sealed record TextToken(string Value) : Token(TokenType.Text, Value);

public sealed record HeaderMarkerToken() : Token(TokenType.HeaderMarker, MarkdownSyntax.HeaderPrefix);

public sealed record ParagraphBreakToken() : Token(TokenType.ParagraphBreak, MarkdownSyntax.ParagraphBreaker);

public abstract record PairedToken(TokenType Type, string Value, bool CanOpen, bool CanClose, bool IsInsideWord)
    : Token(Type, Value);

public sealed record UnderscoreToken(bool CanOpen, bool CanClose, bool IsInsideWord)
    : PairedToken(TokenType.Underscore, MarkdownSyntax.Underscore, CanOpen, CanClose, IsInsideWord);

public sealed record DoubleUnderscoreToken(bool CanOpen, bool CanClose, bool IsInsideWord)
    : PairedToken(TokenType.DoubleUnderscore, MarkdownSyntax.DoubleUnderscore, CanOpen, CanClose, IsInsideWord);

public static class TokenExtensions
{
    public static IEnumerable<IReadOnlyList<Token>> SplitByParagraphBreak(
        this IEnumerable<Token> tokens)
    {
        var current = new List<Token>();
        var hasTokens = false;

        foreach (var token in tokens)
        {
            hasTokens = true;

            if (token is ParagraphBreakToken)
            {
                yield return current;
                current = [];
            }
            else
            {
                current.Add(token);
            }
        }

        if (hasTokens)
            yield return current;
    }
}