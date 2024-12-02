using Markdown.Tags;

namespace Markdown.Tokens;

public struct Token(string content, TokenType tokenType)
{
    public readonly TokenType Type = tokenType;
    public readonly string Content = content;
}