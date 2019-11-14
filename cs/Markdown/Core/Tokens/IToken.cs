namespace Markdown.Core.Tokens
{
    public interface IToken
    {
        int Position { get; }
        int Length { get; }
        string Value { get; }
        TokenType TokenType { get; set; }
    }
}