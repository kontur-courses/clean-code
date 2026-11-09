namespace Markdown.Tokens;

public sealed record TokenType(string Name)
{
    public static readonly TokenType Text = new("text");
    public static readonly TokenType Underscore = new("underscore");
    public static readonly TokenType DoubleUnderscore = new("double_underscore");
    public static readonly TokenType HeaderMarker = new("header_marker");
    public static readonly TokenType ParagraphBreak = new("paragraph_break");
    
}

public sealed record Token(TokenType Type, string Value);
