namespace Markdown
{
    public interface IConverterToken
    {
        void RegisterNested(string tokenType, IConverterToken converterToken);
        Token MakeConverter(string text);
    }
}