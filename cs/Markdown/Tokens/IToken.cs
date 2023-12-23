namespace Markdown.Tokens;

public interface IToken
{
    string Content { get; set; }
    TokenType Type { get; set; }
    int StartPosition { get; set; }
}
