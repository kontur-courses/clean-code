namespace MarkdownParser.Infrastructure.Tokenization.Abstract
{
    public interface ITokenBuilder
    {
        string TokenSymbol { get; }
        Token Create(string raw, int startIndex);
        bool CanCreate(string raw, int startIndex);
    }
}