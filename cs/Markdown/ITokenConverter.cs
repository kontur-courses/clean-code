namespace Markdown
{
    public interface ITokenConverter
    {
        string Convert(Token token, IConverter converter);
    }
}