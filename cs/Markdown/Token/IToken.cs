namespace Markdown.Token;

public interface IToken
{
    int Position { get; }
    int Length { get; }
    TagType Type { get; }
}