namespace Markdown;

public abstract class Token(
    TokenType type, string? content = null, List<Token>? childrens = null)
{
    public readonly string? Content = content;
    public readonly TokenType Type = type;
    public readonly List<Token>? Childs = childrens;
}
