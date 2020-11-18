namespace Markdown
{
    public interface ITokenConverterFactory
    {
        ITagTokenConverter GetTokenConverter(TokenType tokenType);
    }
}