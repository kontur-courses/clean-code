namespace Markdown.Tokens;

public sealed record TokenType(string Name)
{
    public static readonly TokenType Text = new("text");
    public static readonly TokenType Underscore = new("underscore");
    public static readonly TokenType DoubleUnderscore = new("double_underscore");
    public static readonly TokenType HeaderMarker = new("header_marker");
    public static readonly TokenType ParagraphBreak = new("paragraph_break");
    
}

public sealed record Token(TokenType Type, string Value, bool CanOpen = false, bool CanClose = false)
{
    public static Token Text(string value) => new(TokenType.Text, value);
    public static Token Underscore(bool canOpen, bool canClose) => new(TokenType.Underscore, "_", canOpen, canClose);

    public static Token DoubleUnderscore(bool canOpen, bool canClose) =>
        new(TokenType.DoubleUnderscore, "__", canOpen, canClose);

    public const string HeaderPrefix = "# ";
    public static Token HeaderMarker() => new(TokenType.HeaderMarker, HeaderPrefix);

    public static Token ParagraphBreak() => new(TokenType.ParagraphBreak, "\n");

    public static readonly (string Pattern, Func<bool, bool, Token> Create)[] Delimiters = [
        ("__", DoubleUnderscore),
        ("_", Underscore)
    
    ];

}




