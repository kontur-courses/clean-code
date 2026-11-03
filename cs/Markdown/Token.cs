namespace Markdown;

public enum TokenKind
{
    Text,
    Underscore,
    DoubleUnderscore,
    Link
}

public enum TokenRole
{
    Literal,
    Opening,
    Closing
}

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

public sealed class LinkToken : Token
{
    public LinkToken(string raw, int start, string text, string url, List<Token> content)
        : base(TokenKind.Link, raw, start)
    {
        Text = text;
        Url = url;
        Content = content;
    }
    
    public string Text { get; }

    public string Url { get; }
    
    public List<Token> Content { get; }
}