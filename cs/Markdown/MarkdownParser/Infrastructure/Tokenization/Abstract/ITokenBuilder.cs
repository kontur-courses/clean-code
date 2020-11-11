using MarkdownParser.Infrastructure.Tokenization.Models;

namespace MarkdownParser.Infrastructure.Tokenization.Abstract
{
    public interface ITokenBuilder
    {
        string TokenSymbol { get; }
        Token Create(TokenizationContext context);
        bool CanCreate(TokenizationContext context);
    }
}