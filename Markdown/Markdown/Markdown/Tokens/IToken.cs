namespace Markdown.Tokens
{
    public interface IToken
    {
        int Length { get; }
        int Position { get; }
    }
}
