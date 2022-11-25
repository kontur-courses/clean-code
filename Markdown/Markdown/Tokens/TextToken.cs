namespace Markdown.Tokens;

public class TextToken : Token
{
    public TextToken() : base(string.Empty, string.Empty, TokenType.Text) { }
}