namespace Markdown.Tokens;

public sealed record TokenType(string Name)
{
    public static readonly TokenType Text = new("text");
    public static readonly TokenType Underscore = new("underscore");
    public static readonly TokenType DoubleUnderscore = new("double_underscore");
    public static readonly TokenType HeaderMarker = new("header_marker");
    public static readonly TokenType ParagraphBreak = new("paragraph_break");
    
}

public abstract record Token(TokenType Type, string Value)
{
    public static readonly (string Pattern, Func<bool, bool, PairedToken> Create)[] Delimiters =
    [
        ("__", (canOpen, canClose) => new DoubleUnderscoreToken(canOpen, canClose)),
        ("_", (canOpen, canClose) => new UnderscoreToken(canOpen, canClose))
    ];

    public const string HeaderPrefix = "# ";
    public const string ParagraphBreaker = "\n";

}

public sealed record TextToken(string Value) : Token(TokenType.Text, Value);

public sealed record HeaderMarkerToken() : Token(TokenType.HeaderMarker, HeaderPrefix);

public sealed record ParagraphBreakToken() : Token(TokenType.ParagraphBreak, ParagraphBreaker);

public abstract record PairedToken(TokenType Type, string Value, bool CanOpen, bool CanClose)
    : Token(Type, Value);

public sealed record UnderscoreToken(bool CanOpen, bool CanClose)
    : PairedToken(TokenType.Underscore, "_", CanOpen, CanClose);

public sealed record DoubleUnderscoreToken(bool CanOpen, bool CanClose)
    : PairedToken(TokenType.DoubleUnderscore, "__", CanOpen, CanClose);
    