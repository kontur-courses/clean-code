using Markdown.Tokens;

namespace Markdown.Converters
{
    public interface ITokenConverterFactory
    {
        ITokenConverter GetTokenConverter(TokenType tokenType, IConverter converter);
    }
}