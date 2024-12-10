namespace Markdown.Tokens;

public class BaseToken
{
    public TokenType Type { get; }
    public string Content { get; set; }
    public List<BaseToken> Children { get; init; }
    public int HeaderLevel { get; init; }

    public BaseToken(TokenType type, string content, List<BaseToken>? children = null)
    {
        Type = type;
        Content = content;
        Children = children ?? [];
        HeaderLevel = 1;
    }

    public BaseToken(TokenType type) : this(type, string.Empty) { }
    public BaseToken(TokenType type, List<BaseToken>? children = null)
        : this(type, string.Empty, children) { }
}