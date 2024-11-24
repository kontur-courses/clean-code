namespace Markdown.Tokens;

public interface IToken
{
    public string Value { get; }
    public int Length { get; }
}