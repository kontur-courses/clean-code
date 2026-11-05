namespace Markdown.Tokens;

public class Token
{
    public Token(TokenKind kind, string value, int start)
    {
        Kind = kind;
        Value = value;
        Start = start;
    }

    public TokenKind Kind { get; }
    public string Value { get; }
    
    public int Start { get; }
    
    public int End => Start + Value.Length;

    public TokenRole Role { get; set; } = TokenRole.Literal;

    public bool IsDelimiter => Kind == TokenKind.Underscore || Kind == TokenKind.DoubleUnderscore;
}