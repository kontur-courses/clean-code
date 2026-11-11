namespace Markdown.Tokens;

public class Token
{
    public const char Boundary = '\0';

    public Token(TokenKind kind, string value)
    {
        Kind = kind;
        Value = value;
    }

    public TokenKind Kind { get; }

    public string Value { get; }

    public char Before { get; init; } = Boundary;

    public char After { get; init; } = Boundary;

    public TokenRole Role { get; set; } = TokenRole.None;

    /// <summary>Индекс парной лексемы в списке токенов; -1 - пары нет.</summary>
    public int PairIndex { get; set; } = -1;

    public bool IsMarkup => Kind != TokenKind.Text && Role != TokenRole.None;

    public void Deactivate()
    {
        Role = TokenRole.None;
        PairIndex = -1;
    }
}
