namespace Markdown
{
    public interface IConverter
    {
        void Register(string tokenType, IConverterToken converterToken);
        string ConverterTokenToHtml(Token token);
    }
}