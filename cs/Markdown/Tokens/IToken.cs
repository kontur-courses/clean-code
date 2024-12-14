namespace Markdown.Tokens;

public interface IToken
{
    TokenType Type { get; }

    string Content { get; }
}