namespace Markdown.Tokens;

public class Token
{
    public const char Boundary = '\0';

    public Token(TokenKind kind, string value, int position)
    {
        Kind = kind;
        Value = value;
        Position = position;
    }

    public TokenKind Kind { get; }

    public string Value { get; }

    /// <summary>Смещение от начала содержимого блока(не документа).</summary>
    public int Position { get; }

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