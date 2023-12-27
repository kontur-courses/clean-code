using Markdown.Tags;

namespace Markdown.Tokens;

public class Token
{
    public TokenType Type;
    public  string? Content;
    public readonly Tag? Tag;
    public Token(string? content, Tag? tag, TokenType type)
    {
        Content = content;
        Type = type;
        Tag = tag;
    }
}