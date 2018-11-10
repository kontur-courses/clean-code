namespace Markdown
{
    public interface ITagTranslator
    {
        string Translate(string tagBody, string tagSymbol);
    }
}