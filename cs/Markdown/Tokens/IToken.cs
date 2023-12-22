namespace Markdown.Tokens;

public interface IToken
{
    public string Content { get; set; }
    public TokenType Type { get; set; }
    public int StartPosition { get; set; }
}
