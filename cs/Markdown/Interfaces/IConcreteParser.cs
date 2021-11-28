using Markdown;

namespace Markdow.Interfaces
{
    public interface IConcreteParser
    {
        bool CanParse(TokenType tokenType);

        TokenTree ParseToken(int position);
    }
}