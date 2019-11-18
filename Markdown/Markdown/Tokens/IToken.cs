namespace Markdown.Tokens
{
    public interface IToken
    {
        int IndexTokenStart { get; }
        string Text { get; }
        IToken[] NestedTokens { get; }
        int Length { get; }
    }
}