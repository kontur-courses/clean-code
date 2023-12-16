namespace Markdown.Token;

public class Token
{
    public TokenType Type { get; private set; }
    public string Text { get; private set; }

    public Token(TokenType type, string text)
    {
        Type = type;
        Text = text;
    }
}