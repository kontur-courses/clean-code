namespace Markdown.Converters
{
    public interface ITokenConverterFactory
    {
        ITagTokenConverter GetTokenConverter(TokenType tokenType, IConverter converter);
    }
}