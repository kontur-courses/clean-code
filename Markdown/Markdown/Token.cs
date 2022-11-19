namespace Markdown;

public class Token
{
    public Token(string text, TokenType type, bool isSingle = false, bool isOpening = true)
    {
        Text = text;
        TokensType = type;
        IsOpening = isOpening;
        IsSingle = isSingle;
    }

    public string Text { get; set; }
    public TokenType TokensType { get; set; }
    public bool IsOpening { get; set; }
    public bool IsSingle { get; set; }
}